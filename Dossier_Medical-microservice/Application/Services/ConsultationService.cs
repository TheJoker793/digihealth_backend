using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.DTOs.Responses;
using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Domain.ValueObjects;
using static Dossier_Medical_microservice.Application.Exceptions.AppExceptions;

namespace Dossier_Medical_microservice.Application.Services
{
    public class ConsultationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventPublisher _publisher;
        private readonly IDossierCacheService _cache;
        private readonly ILogger<ConsultationService> _logger;

        public ConsultationService(
            IUnitOfWork uow,
            IEventPublisher publisher,
            IDossierCacheService cache,
            ILogger<ConsultationService> logger)
        {
            _uow = uow;
            _publisher = publisher;
            _cache = cache;
            _logger = logger;
        }

        // ── Créer une consultation ───────────────────────────
        public async Task<ConsultationResponse> CreerConsultationAsync(
            CreerConsultationRequest req, CancellationToken ct)
        {
            var dossier = await _uow.Dossiers.GetByIdAsync(req.DossierId, ct)
                ?? throw new NotFoundException($"Dossier {req.DossierId} introuvable.");

            var consultation = dossier.AddConsultation(
                req.RendezVousId ?? Guid.Empty,
                req.TypeConsultation,
                req.Motif);

            await _uow.Dossiers.UpdateAsync(dossier, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.InvalidateAsync(req.DossierId, ct);

            foreach (var evt in dossier.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            dossier.ClearDomainEvents();

            _logger.LogInformation(
                "Consultation {ConsId} créée pour dossier {DossierId}",
                consultation.Id, req.DossierId);

            return ToResponse(consultation);
        }

        // ── Obtenir par ID ───────────────────────────────────
        public async Task<ConsultationResponse> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var c = await _uow.Consultations.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Consultation {id} introuvable.");
            return ToResponse(c);
        }

        // ── Historique par dossier ───────────────────────────
        public async Task<IEnumerable<ConsultationResponse>> GetHistoriqueAsync(
            Guid dossierId, CancellationToken ct)
        {
            var consultations = await _uow.Consultations.GetByDossierAsync(dossierId, ct);
            return consultations.OrderByDescending(c => c.Date).Select(ToResponse);
        }

        // ── Mettre à jour l'examen clinique ──────────────────
        public async Task UpdateExamenCliniqueAsync(
    Guid id,
    UpdateExamenCliniqueRequest req,
    CancellationToken ct)
        {
            var consultation = await _uow.Consultations.GetByIdAsync(id, ct);

            if (consultation is null)
                throw new NotFoundException($"Consultation {id} introuvable.");

            var examenClinique = ExamenClinique.Create(
                req.Poids,
                req.Taille,
                req.TA,
                req.Pouls,
                req.Temperature,
                req.Observations);

            consultation.UpdateExamenClinique(examenClinique);

            await _uow.Consultations.UpdateAsync(consultation, ct);

            await _uow.SaveChangesAsync(ct);
        }

        // ── Clôturer ─────────────────────────────────────────
        public async Task CloturerConsultationAsync(
            Guid id, CloturerConsultationRequest req, CancellationToken ct)
        {
            var c = await _uow.Consultations.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Consultation {id} introuvable.");

            c.Cloturer(req.Conclusion);
            await _uow.Consultations.UpdateAsync(c, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.InvalidateAsync(c.DossierId, ct);

            foreach (var evt in c.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            c.ClearDomainEvents();
        }

        // ── Mapper ───────────────────────────────────────────
        private static ConsultationResponse ToResponse(Domain.Entities.Consultation c) => new(
            c.Id, c.DossierId, c.Date, c.TypeConsultation, c.Motif, c.Statut,
            new ExamenCliniqueResponse(
                c.ExamenClinique.Poids, c.ExamenClinique.Taille, c.ExamenClinique.TA,
                c.ExamenClinique.Pouls, c.ExamenClinique.Temperature,
                c.ExamenClinique.IMC(), c.ExamenClinique.Observations),
            c.Conclusion,
            c.Diagnostics.Select(d => new DiagnosticResponse(
                d.Id, d.CodeCIM11, d.LibelleCIM11, d.Type, d.Commentaire,
                d.EstConfirme, d.CreatedAt)),
            c.Ordonnances.Select(o => new OrdonnanceResponse(
                o.Id, o.Date, o.ValiditeJours, o.IsExpired(), o.Instructions,
                o.Lignes.Select(l => new LigneOrdonnanceResponse(
                    l.MedicamentId, l.NomMedicament, l.Posologie, l.DureeJours, l.Quantite)))));
    }
}
