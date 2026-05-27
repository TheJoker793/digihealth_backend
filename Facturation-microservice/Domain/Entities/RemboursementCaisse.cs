using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Domain.Entities
{
    public class RemboursementCaisse : BaseEntity
    {
        public Guid FactureId { get; set; }
        public Facture? Facture { get; set; }

        public ModeReglement ModeReglement { get; set; }

        public  string NumeroAffilie { get; set; }

        public decimal MontantCaisse { get; set; }

        public decimal MontantComplementaire { get; set; }

        public StatutRemboursement Statut { get; set; }

        // =========================
        // Méthodes métier
        // =========================

        public void Calculer(IEnumerable<LigneFacture> lignes)
        {
            MontantCaisse = lignes.Sum(l => l.PartCaisse());

            MontantComplementaire = lignes.Sum(l => l.PartPatient());
        }
    }
}