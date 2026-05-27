namespace Facturation_microservice.Domain.Events
{
    public record FactureValideeEvent(
        Guid FactureId,
        Guid PatientId,
        DateTimeOffset OccurredAt)
    {
        public FactureValideeEvent(Guid factureId, Guid patientId)
            : this(factureId, patientId, DateTimeOffset.UtcNow) { }
    }
}