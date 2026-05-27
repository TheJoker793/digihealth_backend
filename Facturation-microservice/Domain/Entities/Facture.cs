using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Domain.Entities
{
    public class Facture : BaseEntity
    {
        public required string NumeroFacture { get; set; }

        public Guid PatientId { get; set; }

        public Guid MedecinId { get; set; }

        public Guid? ConsultationId { get; set; }

        public DateOnly DateFacture { get; set; }

        public DateOnly DateEcheance { get; set; }

        public StatutFacture Statut { get; set; }

        public ModeReglement? ModeReglement { get; set; }

        public decimal TauxTVA { get; set; }

        public decimal Remise { get; set; }
        public ICollection<LigneFacture> LignesFacture { get; set; }= new List<LigneFacture>();

        // =========================
        // Méthodes métier
        // =========================

        public static Facture Create(
            string numeroFacture,
            Guid patientId,
            Guid medecinId,
            DateOnly dateFacture,
            DateOnly dateEcheance,
            decimal tauxTVA,
            decimal remise,
            Guid? consultationId = null)
        {
            return new Facture
            {
                NumeroFacture = numeroFacture,
                PatientId = patientId,
                MedecinId = medecinId,
                ConsultationId = consultationId,
                DateFacture = dateFacture,
                DateEcheance = dateEcheance,
                TauxTVA = tauxTVA,
                Remise = remise,
                Statut = StatutFacture.Brouillon
            };
        }

        public void Valider()
        {
            Statut = StatutFacture.Valide;
        }

        public void Payer(ModeReglement mode, decimal montant)
        {
            ModeReglement = mode;
            Statut = StatutFacture.Payee;
        }

        public void Annuler(string motif)
        {
            Statut = StatutFacture.Annulee;
        }

        public decimal MontantTTC()
        {
            return 0;
        }

        public decimal MontantPatient()
        {
            return 0;
        }
    }
}