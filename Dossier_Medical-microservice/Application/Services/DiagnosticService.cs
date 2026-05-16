using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.DTOs.Responses;
using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Interfaces;
using static Dossier_Medical_microservice.Application.Exceptions.AppExceptions;

namespace Dossier_Medical_microservice.Application.Services
{
    public class DiagnosticService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICim11Service _cim11;
        private readonly ILogger<DiagnosticService> _logger;

        public DiagnosticService(
            IUnitOfWork uow,
            ICim11Service cim11,
            ILogger<DiagnosticService> logger)
        {
            _uow = uow;
            _cim11 = cim11;
            _logger = logger;
        }

        // ── Ajouter un diagnostic ────────────────────────────
        public async Task<DiagnosticResponse> AddDiagnosticAsync(
            Guid id, AddDiagnosticRequest req, CancellationToken ct)
        {
            var consultation = await _uow.Consultations.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Consultation {id} introuvable.");

            // Enrichir le libellé via CIM-11 local Docker si non fourni
            var libelle = req.LibelleCIM11;
            if (string.IsNullOrWhiteSpace(libelle))
            {
                var detail = await _cim11.GetByCodeAsync(req.CodeCIM11, ct);
                libelle = detail?.Libelle
                    ?? throw new ValidationException($"Code CIM-11 '{req.CodeCIM11}' introuvable.");
            }

            var diagnostic = consultation.AddDiagnostic(
                req.CodeCIM11, libelle, req.TypeDiagnostic, req.Commentaire);

            await _uow.Consultations.UpdateAsync(consultation, ct);
            await _uow.SaveChangesAsync(ct);

            return new DiagnosticResponse(
                diagnostic.Id, diagnostic.CodeCIM11, diagnostic.LibelleCIM11,
                diagnostic.Type, diagnostic.Commentaire, diagnostic.EstConfirme,
                diagnostic.CreatedAt);
        }

        // ── Obtenir diagnostics d'une consultation ───────────
        public async Task<IEnumerable<DiagnosticResponse>> GetByConsultationAsync(
            Guid consultationId, CancellationToken ct)
        {
            var diagnostics = await _uow.Diagnostics.GetByConsultationAsync(consultationId, ct);
            return diagnostics.Select(d => new DiagnosticResponse(
                d.Id, d.CodeCIM11, d.LibelleCIM11, d.Type,
                d.Commentaire, d.EstConfirme, d.CreatedAt));
        }

        // ── Confirmer un diagnostic ──────────────────────────
        public async Task ConfirmerAsync(Guid diagnosticId, CancellationToken ct)
        {
            var d = await _uow.Diagnostics.GetByIdAsync(diagnosticId, ct)
                ?? throw new NotFoundException($"Diagnostic {diagnosticId} introuvable.");

            d.Confirmer();
            await _uow.Diagnostics.UpdateAsync(d, ct);
            await _uow.SaveChangesAsync(ct);
        }

        // ── Modifier un diagnostic ───────────────────────────
        public async Task ModifierAsync(
            Guid diagnosticId, string libelle, string? commentaire, CancellationToken ct)
        {
            var d = await _uow.Diagnostics.GetByIdAsync(diagnosticId, ct)
                ?? throw new NotFoundException($"Diagnostic {diagnosticId} introuvable.");

            d.Modifier(libelle, commentaire);
            await _uow.Diagnostics.UpdateAsync(d, ct);
            await _uow.SaveChangesAsync(ct);
        }

        // ── Supprimer un diagnostic ──────────────────────────
        public async Task DeleteAsync(Guid diagnosticId, CancellationToken ct)
        {
            var d = await _uow.Diagnostics.GetByIdAsync(diagnosticId, ct)
                ?? throw new NotFoundException($"Diagnostic {diagnosticId} introuvable.");

            if (d.EstConfirme)
                throw new ConflictException("Un diagnostic confirmé ne peut pas être supprimé.");

            await _uow.Diagnostics.DeleteAsync(diagnosticId, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
