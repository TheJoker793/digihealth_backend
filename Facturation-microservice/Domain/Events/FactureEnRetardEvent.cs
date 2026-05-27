namespace Facturation_microservice.Domain.Events
{
    public record FactureEnRetardEvent(
        Guid FactureId,
        Guid PatientId,
        DateTimeOffset OccurredAt)
    {
        public FactureEnRetardEvent(Guid factureId, Guid patientId)
            : this(factureId, patientId, DateTimeOffset.UtcNow) { }
    }
}