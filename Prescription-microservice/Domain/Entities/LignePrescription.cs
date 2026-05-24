using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Domain.Entities
{
    public class LignePrescription : BaseEntity
    {
        public Guid PrescriptionId { get; private set; }
        public Guid MedicamentId { get; private set; }
        public string NomMedicament { get; private set; } = default!;
        public string DCI { get; private set; } = default!;  // Dénomination Commune Internationale
        public Posologie Posologie { get; private set; } = default!;
        public int DureeJours { get; private set; }
        public int Quantite { get; private set; }
        public bool Renouvellement { get; private set; }
        public int NbRenouvellements { get; private set; }
        public string? Commentaire { get; private set; }

        private LignePrescription() { }

        public static LignePrescription Create(
            Guid prescriptionId,
            Guid medicamentId,
            string nomMedicament,
            string dci,
            Posologie posologie,
            int dureeJours,
            int quantite,
            bool renouvellement = false,
            int nbRenouvellements = 0,
            string? commentaire = null)
        {
            if (dureeJours < 1)
                throw new ArgumentException("La durée doit être supérieure à 0 jour.");
            if (quantite < 1)
                throw new ArgumentException("La quantité doit être supérieure à 0.");

            return new LignePrescription
            {
                PrescriptionId = prescriptionId,
                MedicamentId = medicamentId,
                NomMedicament = nomMedicament.Trim(),
                DCI = dci.Trim(),
                Posologie = posologie,
                DureeJours = dureeJours,
                Quantite = quantite,
                Renouvellement = renouvellement,
                NbRenouvellements = renouvellement ? nbRenouvellements : 0,
                Commentaire = commentaire?.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        // Texte lisible pour le PDF
        public string ToOrdonnanceText()
            => $"{NomMedicament} ({DCI}) — {Posologie} — {DureeJours} jours — Qté: {Quantite}" +
               (Renouvellement ? $" — Renouvelable {NbRenouvellements}x" : "");
    }
}
