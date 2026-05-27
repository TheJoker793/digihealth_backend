using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Domain.Entities
{
    public class Paiement : BaseEntity
    {
        public Guid FactureId { get; set; }

        public DateTimeOffset Date { get; set; }

        public decimal Montant { get; set; }

        public ModeReglement Mode { get; set; }

        public string? Reference { get; set; }

        public Guid Caissier { get; set; }

        // =========================
        // Factory Method
        // =========================
        public static Paiement Create(
            Guid factureId,
            decimal montant,
            ModeReglement mode,
            Guid caissier,
            string? reference = null)
        {
            return new Paiement
            {
                FactureId = factureId,
                Montant = montant,
                Mode = mode,
                Caissier = caissier,
                Reference = reference,
                Date = DateTimeOffset.UtcNow
            };
        }
    }
}