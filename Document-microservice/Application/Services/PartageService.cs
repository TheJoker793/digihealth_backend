using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Domain.Exceptions;
using Document_microservice.Domain.Interfaces;

namespace Document_microservice.Application.Services
{
    public class PartageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IStorageService _storage;

        public PartageService(IUnitOfWork uow, IStorageService storage)
        {
            _uow = uow;
            _storage = storage;
        }

        // ═══════════════════════════════════════════
        // Créer un partage
        // ═══════════════════════════════════════════
        public async Task<PartageResponse> CreerPartageAsync(
            CreerPartageRequest request,
            CancellationToken ct = default)
        {
            var document = await _uow.Documents.GetAvecPartagesAsync(request.DocumentId, ct)
                ?? throw new DocumentNotFoundException(request.DocumentId);

            if (document.EstArchive)
                throw new DocumentArchiveException(request.DocumentId);

            var partage = document.Partager(
                request.DestinataireId,
                request.TypeDestinataire,
                request.LectureSeule,
                request.DateExpiration);

            _uow.Documents.Update(document);
            await _uow.SaveChangesAsync(ct);

            return ToResponse(partage);
        }

        // ═══════════════════════════════════════════
        // Accès via token (portail patient / partage externe)
        // ═══════════════════════════════════════════
        public async Task<UrlPresigneeResponse> AccederParTokenAsync(
            string token,
            CancellationToken ct = default)
        {
            var partage = await _uow.Partages.GetByTokenAsync(token, ct)
                ?? throw new PartageExpireException(token);

            if (!partage.EstValide())
                throw new PartageExpireException(token);

            // Récupérer la version active du document
            var document = await _uow.Documents.GetAvecVersionsAsync(partage.DocumentMedicalId, ct)
                ?? throw new DocumentNotFoundException(partage.DocumentMedicalId);

            var version = document.VersionActive
                ?? throw new VersionIntrouvableException(document.Id, 0);

            // Enregistrer l'accès
            partage.EnregistrerAcces();
            _uow.Partages.Update(partage);
            await _uow.SaveChangesAsync(ct);

            // Générer URL pré-signée
            var duree = TimeSpan.FromMinutes(15);
            var url = await _storage.GenererUrlPresigneeAsync(version.CheminFichier, duree, ct);

            return new UrlPresigneeResponse(
                url,
                version.NomFichier,
                version.Format,
                version.TailleOctets,
                DateTime.UtcNow.Add(duree));
        }

        // ═══════════════════════════════════════════
        // Révoquer un partage
        // ═══════════════════════════════════════════
        public async Task RevoquerAsync(Guid partageId, CancellationToken ct = default)
        {
            var partage = await _uow.Partages.GetByIdAsync(partageId, ct)
                ?? throw new KeyNotFoundException($"Partage {partageId} introuvable.");

            partage.Revoquer();
            _uow.Partages.Update(partage);
            await _uow.SaveChangesAsync(ct);
        }

        // ═══════════════════════════════════════════
        // Liste des partages d'un document
        // ═══════════════════════════════════════════
        public async Task<IEnumerable<PartageResponse>> GetByDocumentAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            var partages = await _uow.Partages.GetByDocumentAsync(documentId, ct);
            return partages.Select(ToResponse);
        }

        // ═══════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════
        private static PartageResponse ToResponse(Domain.Entities.PartageDocument p)
            => new(p.Id, p.DocumentMedicalId, p.TokenAcces,
                   p.DestinataireId, p.TypeDestinataire, p.LectureSeule,
                   p.NombreAcces, p.LimiteAcces, p.DateExpiration,
                   p.EstValide(), p.CreatedAt);
    }
}
