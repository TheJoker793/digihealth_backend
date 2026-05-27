namespace Facturation_microservice.Domain.Events
{
    public record FacturePayeeEvent(
        Guid FactureId,
        Guid PatientId,
        DateTimeOffset OccurredAt)
    {
        public FacturePayeeEvent(Guid factureId, Guid patientId)
            : this(factureId, patientId, DateTimeOffset.UtcNow) { }
    }
}