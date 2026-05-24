using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Enums;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Application.DTOs.Requests;

namespace Prescription_microservice.Application.Services
{
    public class InteractionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InteractionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ── VERIFIER ──────────────────────────────────────────
        public async Task<IEnumerable<InteractionDetectee>> VerifierAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            return await _unitOfWork.Interactions.GetByPrescriptionAsync(prescription.Id, ct);
        }

        // ── CLASSIFIER SEVERITE ───────────────────────────────
        public SeveriteInteraction ClassifierSeverite(string mecanisme)
        {
            if (mecanisme.Contains("contre", StringComparison.OrdinalIgnoreCase))
                return SeveriteInteraction.ContreIndication;
            if (mecanisme.Contains("avertissement", StringComparison.OrdinalIgnoreCase) ||
                mecanisme.Contains("alerte", StringComparison.OrdinalIgnoreCase))
                return SeveriteInteraction.Avertissement;

            return SeveriteInteraction.Information;
        }

        // ── GENERER RECOMMANDATION ────────────────────────────
        public string GenererRecommandation(SeveriteInteraction severite)
        {
            return severite switch
            {
                SeveriteInteraction.Information =>
                    "Interaction mineure : informer le patient si nécessaire.",
                SeveriteInteraction.Avertissement =>
                    "Interaction significative : confirmation médicale requise avant validation.",
                SeveriteInteraction.ContreIndication =>
                    "Interaction bloquante : justification obligatoire pour contourner.",
                _ => "Recommandation non disponible."
            };
        }

        // ── HAS BLOQUANTE ─────────────────────────────────────
        public async Task<bool> HasBloquanteAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            var interactions = await _unitOfWork.Interactions.GetNonContourneesAsync(prescriptionId, ct);
            return interactions.Any(i => i.IsBloquante());
        }

        // ── CONTOURNER INTERACTION ────────────────────────────
        public async Task ContournerInteractionAsync(ContournerInteractionRequest request, CancellationToken ct = default)
        {
            var interactions = await _unitOfWork.Interactions.GetByPrescriptionAsync(request.InteractionId, ct);
            var interaction = interactions.FirstOrDefault(i => i.Id == request.InteractionId)
                ?? throw new InvalidOperationException("Interaction introuvable.");

            interaction.Contourner(request.Justification, request.MedecinId);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ── AJOUTER INTERACTION ───────────────────────────────
        public async Task<InteractionDetectee> AjouterInteractionAsync(
            Guid prescriptionId,
            string medicamentA,
            string medicamentB,
            string mecanisme,
            CancellationToken ct = default)
        {
            var severite = ClassifierSeverite(mecanisme);
            var recommandation = GenererRecommandation(severite);

            var interaction = InteractionDetectee.Create(
                prescriptionId,
                medicamentA,
                medicamentB,
                severite,
                mecanisme,
                recommandation
            );

            await _unitOfWork.Interactions.AddAsync(interaction, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return interaction;
        }
    }
}
