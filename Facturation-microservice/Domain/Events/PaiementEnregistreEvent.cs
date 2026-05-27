using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Domain.Events
{
    public record PaiementEnregistreEvent(
        Guid FactureId,
        decimal Montant,
        ModeReglement ModeReglement,
        DateTimeOffset OccurredAt)
    {
        public PaiementEnregistreEvent(
            Guid factureId,
            decimal montant,
            ModeReglement modeReglement)
            : this(
                factureId,
                montant,
                modeReglement,
                DateTimeOffset.UtcNow)
        { }
    }
}