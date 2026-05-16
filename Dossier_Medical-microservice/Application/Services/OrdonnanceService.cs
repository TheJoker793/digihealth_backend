using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.DTOs.Responses;
using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Interfaces;
using static Dossier_Medical_microservice.Application.Exceptions.AppExceptions;

namespace Dossier_Medical_microservice.Application.Services
{
    public class OrdonnanceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventPublisher _publisher;
        private readonly ILogger<OrdonnanceService> _logger;

        public OrdonnanceService(
            IUnitOfWork uow,
            IEventPublisher publisher,
            ILogger<OrdonnanceService> logger)
        {
            _uow = uow;
            _publisher = publisher;
            _logger = logger;
        }

        // ── Créer une ordonnance ─────────────────────────────
        public async Task<OrdonnanceResponse> CreateOrdonnanceAsync(
            Guid consultationId, CreateOrdonnanceRequest req, CancellationToken ct)
        {
            var consultation = await _uow.Consultations.GetByIdAsync(consultationId, ct)
                ?? throw new NotFoundException($"Consultation {consultationId} introuvable.");

            var ordonnance = consultation.AddOrdonnance(req.ValiditeJours, req.Instructions);

            // Ajouter les lignes
            foreach (var ligne in req.Lignes)
                ordonnance.AddLigne(
                    ligne.MedicamentId,
                    ligne.NomMedicament,
                    ligne.Posologie,
                    ligne.DureeJours,
                    ligne.Quantite);

            await _uow.Consultations.UpdateAsync(consultation, ct);
            await _uow.SaveChangesAsync(ct);

            foreach (var evt in consultation.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            consultation.ClearDomainEvents();

            _logger.LogInformation(
                "Ordonnance {OrdId} créée pour consultation {ConsId}",
                ordonnance.Id, consultationId);

            return ToResponse(ordonnance);
        }

        // ── Ordonnances d'une consultation ───────────────────
        public async Task<IEnumerable<OrdonnanceResponse>> GetByConsultationAsync(
            Guid consultationId, CancellationToken ct)
        {
            var ordonnances = await _uow.Ordonnances.GetByConsultationAsync(consultationId, ct);
            return ordonnances.Select(ToResponse);
        }

        // ── Ordonnances actives d'un patient ─────────────────
        public async Task<IEnumerable<OrdonnanceResponse>> GetActiveByPatientAsync(
            Guid patientId, CancellationToken ct)
        {
            var ordonnances = await _uow.Ordonnances.GetActiveByPatientAsync(patientId, ct);
            return ordonnances.Select(ToResponse);
        }

        // ── Mapper ───────────────────────────────────────────
        private static OrdonnanceResponse ToResponse(Domain.Entities.Ordonnance o) => new(
            o.Id, o.Date, o.ValiditeJours, o.IsExpired(), o.Instructions,
            o.Lignes.Select(l => new LigneOrdonnanceResponse(
                l.MedicamentId, l.NomMedicament,
                l.Posologie, l.DureeJours, l.Quantite)));
    }
}
