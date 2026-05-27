using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Domain.Entities
{
    public class LigneFacture : BaseEntity
    {
        public Guid FactureId { get; set; }

        public required string Designation { get; set; }

        public TypeActe TypeActe { get; set; }

        public string? CodeActe { get; set; }

        public decimal PrixUnitaire { get; set; }

        public int Quantite { get; set; }

        public decimal TauxRemboursement { get; set; }

        // =========================
        // Méthodes métier
        // =========================

        public decimal MontantHT()
        {
            return PrixUnitaire * Quantite;
        }

        public decimal PartCaisse()
        {
            return MontantHT() * (TauxRemboursement / 100);
        }

        public decimal PartPatient()
        {
            return MontantHT() - PartCaisse();
        }
    }
}