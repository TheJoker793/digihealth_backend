using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Exceptions;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Domain.ValueObjects;

namespace Document_microservice.Application.Services
{
    public class DocumentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IStorageService _storage;
        private readonly IVirusScanService _virusScan;
        private readonly INumeroDocumentGenerator _numeroGen;
        private readonly IEventPublisher _publisher;
        private readonly IDocumentCacheService _cache;

        public DocumentService(
            IUnitOfWork uow,
            IStorageService storage,
            IVirusScanService virusScan,
            INumeroDocumentGenerator numeroGen,
            IEventPublisher publisher,
            IDocumentCacheService cache)
        {
            _uow = uow;
            _storage = storage;
            _virusScan = virusScan;
            _numeroGen = numeroGen;
            _publisher = publisher;
            _cache = cache;
        }

        // ═══════════════════════════════════════════════════════
        // UPLOAD — crée le document + version 1
        // ═══════════════════════════════════════════════════════
        public async Task<DocumentResponse> UploadAsync(
            UploadDocumentRequest request,
            CancellationToken ct = default)
        {
            // 1. Scan antivirus
            await using var stream = request.Fichier.OpenReadStream();
            var scanResult = await _virusScan.ScannerAsync(stream, ct);
            if (!scanResult.EstSain)
                throw new VirusDetecteException(scanResult.NomVirus!);

            // 2. Générer numéro unique
            var numero = await _numeroGen.GenererAsync(request.CabinetId, ct);

            // 3. Créer l'agrégat Domain
            var document = DocumentMedical.Create(
                numero,
                request.Titre,
                request.TypeDocument,
                request.PatientId,
                request.MedecinId,
                request.CabinetId,
                request.ConsultationId,
                request.PrescriptionId,
                request.Description);

            // 4. Uploader le fichier vers MinIO
            var bucket = $"cabinet-{request.CabinetId}";
            var chemin = $"patients/{request.PatientId}/{numero}/v1{Path.GetExtension(request.Fichier.FileName)}";
            var contentType = request.Fichier.ContentType;

            await using var streamUpload = request.Fichier.OpenReadStream();
            await _storage.UploadAsync(streamUpload, chemin, contentType, bucket, ct);

            // 5. Ajouter version 1 à l'agrégat
            var motsCles = request.MotsCles?
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? [];

            var metadonnees = MetadonneeFichier.Create(
                auteur: $"{request.MedecinId}",
                motsCles: motsCles);

            document.AjouterVersion(
                chemin,
                request.Fichier.FileName,
                DetecterFormat(request.Fichier),
                request.Fichier.Length,
                await ComputeChecksumAsync(request.Fichier),
                metadonnees);

            // 6. Persister
            await _uow.Documents.AddAsync(document, ct);
            await _uow.SaveChangesAsync(ct);

            // 7. Publier domain events
            foreach (var evt in document.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            document.ClearDomainEvents();

            return ToResponse(document);
        }

        // ═══════════════════════════════════════════════════════
        // TÉLÉCHARGEMENT — retourne une URL pré-signée MinIO
        // ═══════════════════════════════════════════════════════
        public async Task<UrlPresigneeResponse> TelechargerAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecVersionsAsync(documentId, ct)
                ?? throw new DocumentNotFoundException(documentId);

            var version = document.VersionActive
                ?? throw new VersionIntrouvableException(documentId, 0);

            // Vérifier cache Redis d'abord
            var urlCachee = await _cache.GetUrlPresigneeAsync(version.CheminFichier);
            if (urlCachee is not null)
            {
                return new UrlPresigneeResponse(
                    urlCachee,
                    version.NomFichier,
                    version.Format,
                    version.TailleOctets,
                    DateTime.UtcNow.AddMinutes(15));
            }

            // Générer URL pré-signée valable 15 min
            var duree = TimeSpan.FromMinutes(15);
            var url = await _storage.GenererUrlPresigneeAsync(version.CheminFichier, duree, ct);

            // Mettre en cache
            await _cache.SetUrlPresigneeAsync(version.CheminFichier, url, duree);

            return new UrlPresigneeResponse(
                url,
                version.NomFichier,
                version.Format,
                version.TailleOctets,
                DateTime.UtcNow.Add(duree));
        }

        // ═══════════════════════════════════════════════════════
        // PUBLIER
        // ═══════════════════════════════════════════════════════
        public async Task<DocumentResponse> PublierAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecVersionsAsync(documentId, ct)
                ?? throw new DocumentNotFoundException(documentId);

            document.Publier();
            _uow.Documents.Update(document);
            await _uow.SaveChangesAsync(ct);

            foreach (var evt in document.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            document.ClearDomainEvents();

            return ToResponse(document);
        }

        // ═══════════════════════════════════════════════════════
        // ARCHIVER
        // ═══════════════════════════════════════════════════════
        public async Task ArchiverAsync(Guid documentId, CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetByIdAsync(documentId, ct)
                ?? throw new DocumentNotFoundException(documentId);

            document.Archiver();
            _uow.Documents.Update(document);
            await _uow.SaveChangesAsync(ct);

            await _cache.InvaliderAsync(documentId);

            foreach (var evt in document.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            document.ClearDomainEvents();
        }

        // ═══════════════════════════════════════════════════════
        // LISTE PAR PATIENT
        // ═══════════════════════════════════════════════════════
        public async Task<IEnumerable<DocumentResponse>> GetByPatientAsync(
            Guid patientId,
            CancellationToken ct = default)
        {
            var documents = await _uow.Documents.GetByPatientAsync(patientId, ct);
            return documents.Select(ToResponse);
        }

        // ═══════════════════════════════════════════════════════
        // GET PAR ID
        // ═══════════════════════════════════════════════════════
        public async Task<DocumentResponse> GetByIdAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecVersionsAsync(documentId, ct)
                ?? throw new DocumentNotFoundException(documentId);

            return ToResponse(document);
        }

        // ═══════════════════════════════════════════════════════
        // HELPERS PRIVÉS
        // ═══════════════════════════════════════════════════════
        private static FormatFichier DetecterFormat(IFormFile fichier)
            => fichier.ContentType switch
            {
                "application/pdf" => FormatFichier.PDF,
                "image/jpeg" => FormatFichier.JPEG,
                "image/png" => FormatFichier.PNG,
                "application/dicom" => FormatFichier.DICOM,
                _ => throw new FormatNonAutoriseException(fichier.ContentType)
            };

        private static async Task<string> ComputeChecksumAsync(IFormFile fichier)
        {
            await using var stream = fichier.OpenReadStream();
            var hash = await System.Security.Cryptography.SHA256.HashDataAsync(stream);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static DocumentResponse ToResponse(DocumentMedical d)
        {
            var versionActive = d.VersionActive;
            return new DocumentResponse(
                d.Id, d.Numero, d.Titre, d.Description,
                d.TypeDocument, d.Statut, d.EstArchive,
                d.PatientId, d.MedecinId, d.CabinetId, d.ConsultationId,
                d.CreatedAt, d.UpdatedAt,
                versionActive is null ? null : new VersionDocumentResponse(
                    versionActive.Id,
                    versionActive.NumeroVersion,
                    versionActive.NomFichier,
                    versionActive.Format,
                    versionActive.TailleOctets,
                    versionActive.EstActive,
                    versionActive.Metadonnees.Auteur,
                    versionActive.Metadonnees.NbPages,
                    versionActive.CreatedAt));
        }
    }

}
