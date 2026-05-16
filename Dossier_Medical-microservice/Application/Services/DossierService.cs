using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.DTOs.Responses;
using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Domain.ValueObjects;
using System.Text.Json;
using static Dossier_Medical_microservice.Application.Exceptions.AppExceptions;

namespace Dossier_Medical_microservice.Application.Services
{
    public class DossierService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventPublisher _publisher;
        private readonly IDossierCacheService _cache;
        private readonly INumeroDossierGenerator _numGenerator;
        private readonly ILogger<DossierService> _logger;

        public DossierService(
            IUnitOfWork uow,
            IEventPublisher publisher,
            IDossierCacheService cache,
            INumeroDossierGenerator numGenerator,
            ILogger<DossierService> logger)
        {
            _uow = uow;
            _publisher = publisher;
            _cache = cache;
            _numGenerator = numGenerator;
            _logger = logger;
        }

        // ── Ouvrir un dossier ────────────────────────────────
        public async Task<DossierResponse> OuvrirDossierAsync(
            OuvrirDossierRequest req, CancellationToken ct)
        {
            // Vérifier doublon actif pour ce patient + médecin
            var existeDeja = await _uow.Dossiers.ExistsForPatientAsync(req.PatientId, req.MedecinId, ct);
            if (existeDeja)
                throw new ConflictException(
                    "Un dossier médical ouvert existe déjà pour ce patient avec ce médecin.");

            // Générer le numéro de dossier
            var numero = await _numGenerator.GenerateAsync(req.CabinetId, ct);

            var dossier = DossierMedical.Create(
                req.PatientId,
                req.MedecinId,
                req.CabinetId,
                NumeroDossier.FromString(numero),
                req.Motif,
                req.Anamnese);

            await _uow.Dossiers.AddAsync(dossier, ct);
            await _uow.SaveChangesAsync(ct);

            // Publier les domain events
            foreach (var evt in dossier.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            dossier.ClearDomainEvents();

            _logger.LogInformation(
                "Dossier {Numero} ouvert pour patient {PatientId}",
                numero, req.PatientId);

            return ToResponse(dossier);
        }

        // ── Obtenir par ID ───────────────────────────────────
        public async Task<DossierResponse> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var dossier = await _uow.Dossiers.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Dossier médical {id} introuvable.");
            return ToResponse(dossier);
        }

        // ── Historique par patient ───────────────────────────
        public async Task<IEnumerable<DossierSummaryResponse>> GetByPatientAsync(
            Guid patientId, CancellationToken ct)
        {
            var dossiers = await _uow.Dossiers.GetByPatientAsync(patientId, ct);
            return dossiers.Select(ToSummary);
        }

        // ── Résumé (avec cache Redis) ────────────────────────
        public async Task<ResumeDossierResponse> GetResumeAsync(Guid id, CancellationToken ct)
        {
            // Vérifier le cache
            var cached = await _cache.GetResumeAsync(id, ct);
            if (cached is not null)
                return JsonSerializer.Deserialize<ResumeDossierResponse>(cached)!;

            var dossier = await _uow.Dossiers.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Dossier {id} introuvable.");

            var consultations = await _uow.Consultations.GetByDossierAsync(id, ct);
            var ordonnancesActives = await _uow.Ordonnances.GetActiveByPatientAsync(dossier.PatientId, ct);

            var codesCim11 = consultations
                .SelectMany(c => c.Diagnostics)
                .Select(d => d.CodeCIM11)
                .Distinct()
                .Take(10)
                .ToList();

            var resume = new ResumeDossierResponse(
                DossierId: id,
                NumeroDossier: dossier.NumeroDossier.Valeur,
                NbConsultations: consultations.Count(),
                DerniersCodesCIM11: codesCim11,
                OrdonnancesActives: ordonnancesActives.Select(MapOrdonnance),
                GeneratedAt: DateTimeOffset.UtcNow
            );

            // Mettre en cache 10 minutes
            await _cache.SetResumeAsync(id,
                JsonSerializer.Serialize(resume), TimeSpan.FromMinutes(10), ct);

            return resume;
        }

        // ── Clôturer ─────────────────────────────────────────
        public async Task CloturerAsync(Guid id, CancellationToken ct)
        {
            var dossier = await _uow.Dossiers.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Dossier {id} introuvable.");

            dossier.Cloturer();
            await _uow.Dossiers.UpdateAsync(dossier, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.InvalidateAsync(id, ct);

            foreach (var evt in dossier.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            dossier.ClearDomainEvents();
        }

        // ── Archiver ─────────────────────────────────────────
        public async Task ArchiverAsync(Guid id, CancellationToken ct)
        {
            var dossier = await _uow.Dossiers.GetByIdAsync(id, ct)
                ?? throw new NotFoundException($"Dossier {id} introuvable.");

            dossier.Archiver();
            await _uow.Dossiers.UpdateAsync(dossier, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.InvalidateAsync(id, ct);
        }

        // ── Mappers privés ───────────────────────────────────
        private static DossierResponse ToResponse(DossierMedical d) => new(
            d.Id, d.NumeroDossier.Valeur, d.PatientId, d.MedecinId,
            d.DateOuverture, d.Statut, d.Motif, d.Anamnese,
            d.Consultations.Count, d.Documents.Count);

        private static DossierSummaryResponse ToSummary(DossierMedical d) => new(
            d.Id, d.NumeroDossier.Valeur, d.DateOuverture, d.Statut, d.Motif,
            d.Consultations.Count);

        private static OrdonnanceResponse MapOrdonnance(Domain.Entities.Ordonnance o) => new(
            o.Id, o.Date, o.ValiditeJours, o.IsExpired(), o.Instructions,
            o.Lignes.Select(l => new LigneOrdonnanceResponse(
                l.MedicamentId, l.NomMedicament, l.Posologie, l.DureeJours, l.Quantite)));
    }
}
