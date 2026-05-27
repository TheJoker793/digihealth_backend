namespace Facturation_microservice.Domain.Events
{
    public record FactureAnnuleeEvent(
        Guid FactureId,
        DateTimeOffset OccurredAt)
    {
        public FactureAnnuleeEvent(Guid factureId)
            : this(factureId, DateTimeOffset.UtcNow) { }
    }
}