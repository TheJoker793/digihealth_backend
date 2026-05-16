using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.DTOs.Responses;
using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Interfaces;
using static Dossier_Medical_microservice.Application.Exceptions.AppExceptions;

namespace Dossier_Medical_microservice.Application.Services
{
    public class DocumentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IStorageService _storage;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(
            IUnitOfWork uow,
            IStorageService storage,
            ILogger<DocumentService> logger)
        {
            _uow = uow;
            _storage = storage;
            _logger = logger;
        }

        // ── Upload ───────────────────────────────────────────
        public async Task<DocumentResponse> UploadAsync(
            UploadDocumentRequest req, CancellationToken ct)
        {
            var dossier = await _uow.Dossiers.GetByIdAsync(req.DossierId, ct)
                ?? throw new NotFoundException($"Dossier {req.DossierId} introuvable.");

            // Upload vers MinIO
            var chemin = await _storage.UploadAsync(
                req.Contenu, req.FileName, req.MimeType, ct);

            var doc = dossier.AddDocument(
                req.TypeDocument, req.Titre,
                chemin, req.MimeType,
                req.Contenu.Length);

            await _uow.Dossiers.UpdateAsync(dossier, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Document {Titre} uploadé pour dossier {DossierId}",
                req.Titre, req.DossierId);

            var url = await _storage.GetPresignedUrlAsync(chemin, 15, ct);
            return ToResponse(doc, url);
        }

        // ── Liste documents d'un dossier ─────────────────────
        public async Task<IEnumerable<DocumentResponse>> GetByDossierAsync(
            Guid dossierId,
            Domain.Enums.TypeDocument? type,
            CancellationToken ct)
        {
            var docs = await _uow.Documents.GetByDossierAsync(dossierId, type, ct);
            var responses = new List<DocumentResponse>();

            foreach (var doc in docs)
            {
                var url = await _storage.GetPresignedUrlAsync(doc.CheminFichier, 15, ct);
                responses.Add(ToResponse(doc, url));
            }

            return responses;
        }

        // ── Télécharger (URL pré-signée) ─────────────────────
        public async Task<string> GetDownloadUrlAsync(Guid id, CancellationToken ct)
        {
            var doc = await _uow.Documents.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Document {id} introuvable.");

            return await _storage.GetPresignedUrlAsync(doc.CheminFichier, 15, ct);
        }

        // ── Supprimer (soft delete) ──────────────────────────
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var doc = await _uow.Documents.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Document {id} introuvable.");

            doc.SoftDelete();
            await _uow.Documents.UpdateAsync(doc, ct);
            await _uow.SaveChangesAsync(ct);

            // Supprimer aussi du stockage MinIO
            await _storage.DeleteAsync(doc.CheminFichier, ct);
        }

        // ── Mapper ───────────────────────────────────────────
        private static DocumentResponse ToResponse(
            Domain.Entities.DocumentMedical doc, string? url) => new(
            doc.Id, doc.TypeDocument, doc.Titre,
            doc.DateDocument, doc.MimeType, doc.TailleOctets, url);
    }
}
