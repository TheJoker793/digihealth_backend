using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Exceptions;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Domain.ValueObjects;

namespace Document_microservice.Application.Services
{
    public class VersionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IStorageService _storage;
        private readonly IVirusScanService _virusScan;

        public VersionService(
            IUnitOfWork uow,
            IStorageService storage,
            IVirusScanService virusScan)
        {
            _uow = uow;
            _storage = storage;
            _virusScan = virusScan;
        }

        // ═══════════════════════════════════════════
        // Ajouter une nouvelle version
        // ═══════════════════════════════════════════
        public async Task<VersionDocumentResponse> AjouterVersionAsync(
            AjouterVersionRequest request,
            Guid medecinId,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecVersionsAsync(request.DocumentId, ct)
                ?? throw new DocumentNotFoundException(request.DocumentId);

            if (document.EstArchive)
                throw new DocumentArchiveException(request.DocumentId);

            // Scan antivirus
            await using var stream = request.Fichier.OpenReadStream();
            var scan = await _virusScan.ScannerAsync(stream, ct);
            if (!scan.EstSain)
                throw new VirusDetecteException(scan.NomVirus!);

            // Upload nouveau fichier
            var nouveauNumero = document.Versions.Count + 1;
            var ext = Path.GetExtension(request.Fichier.FileName);
            var chemin = $"patients/{document.PatientId}/{document.Numero}/v{nouveauNumero}{ext}";
            var bucket = $"cabinet-{document.CabinetId}";

            await using var streamUpload = request.Fichier.OpenReadStream();
            await _storage.UploadAsync(streamUpload, chemin, request.Fichier.ContentType, bucket, ct);

            var metadonnees = MetadonneeFichier.Create(auteur: $"{medecinId}");
            var format = request.Fichier.ContentType switch
            {
                "application/pdf" => FormatFichier.PDF,
                "image/jpeg" => FormatFichier.JPEG,
                "image/png" => FormatFichier.PNG,
                "application/dicom" => FormatFichier.DICOM,
                _ => throw new FormatNonAutoriseException(request.Fichier.ContentType)
            };

            // Calcul checksum
            await using var streamCheck = request.Fichier.OpenReadStream();
            var hash = await System.Security.Cryptography.SHA256.HashDataAsync(streamCheck);
            var checksum = Convert.ToHexString(hash).ToLowerInvariant();

            var version = document.AjouterVersion(
                chemin,
                request.Fichier.FileName,
                format,
                request.Fichier.Length,
                checksum,
                metadonnees);

            _uow.Documents.Update(document);
            await _uow.SaveChangesAsync(ct);

            return new VersionDocumentResponse(
                version.Id,
                version.NumeroVersion,
                version.NomFichier,
                version.Format,
                version.TailleOctets,
                version.EstActive,
                version.Metadonnees.Auteur,
                version.Metadonnees.NbPages,
                version.CreatedAt);
        }

        // ═══════════════════════════════════════════
        // Historique des versions
        // ═══════════════════════════════════════════
        public async Task<IEnumerable<VersionDocumentResponse>> GetHistoriqueAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            var versions = await _uow.Versions.GetByDocumentAsync(documentId, ct);

            return versions
                .OrderByDescending(v => v.NumeroVersion)
                .Select(v => new VersionDocumentResponse(
                    v.Id, v.NumeroVersion, v.NomFichier,
                    v.Format, v.TailleOctets, v.EstActive,
                    v.Metadonnees.Auteur, v.Metadonnees.NbPages,
                    v.CreatedAt));
        }

        // ═══════════════════════════════════════════
        // Restaurer une version précédente
        // ═══════════════════════════════════════════
        public async Task RestaurerVersionAsync(
            Guid documentId,
            int numeroVersion,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecVersionsAsync(documentId, ct)
                ?? throw new DocumentNotFoundException(documentId);

            if (document.EstArchive)
                throw new DocumentArchiveException(documentId);

            document.RevenirAVersion(numeroVersion);
            _uow.Documents.Update(document);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
