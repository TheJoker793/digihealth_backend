using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Domain.Entities
{
    public class InteractionDetectee : BaseEntity
    {
        public Guid PrescriptionId { get; private set; }
        public string MedicamentA { get; private set; } = default!;
        public string MedicamentB { get; private set; } = default!;
        public SeveriteInteraction Severite { get; private set; }
        public string Mecanisme { get; private set; } = default!;
        public string Recommandation { get; private set; } = default!;
        public bool EstContournee { get; private set; }
        public string? Justification { get; private set; }
        public Guid? ContourneePar { get; private set; }  // MedecinId
        public DateTimeOffset? ContourneeAt { get; private set; }

        private InteractionDetectee() { }

        public static InteractionDetectee Create(
            Guid prescriptionId,
            string medicamentA,
            string medicamentB,
            SeveriteInteraction severite,
            string mecanisme,
            string recommandation)
        {
            return new InteractionDetectee
            {
                PrescriptionId = prescriptionId,
                MedicamentA = medicamentA.Trim(),
                MedicamentB = medicamentB.Trim(),
                Severite = severite,
                Mecanisme = mecanisme.Trim(),
                Recommandation = recommandation.Trim(),
                EstContournee = false,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public void Contourner(string justification, Guid medecinId)
        {
            if (Severite != SeveriteInteraction.ContreIndication)
                throw new InvalidOperationException(
                    "Seules les contre-indications peuvent être contournées.");

            if (string.IsNullOrWhiteSpace(justification) || justification.Trim().Length < 20)
                throw new ArgumentException(
                    "La justification doit contenir au moins 20 caractères.");

            EstContournee = true;
            Justification = justification.Trim();
            ContourneePar = medecinId;
            ContourneeAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public bool IsBloquante() => Severite == SeveriteInteraction.ContreIndication && !EstContournee;
    }
}
