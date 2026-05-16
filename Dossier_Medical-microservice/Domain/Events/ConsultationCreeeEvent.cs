namespace Dossier_Medical_microservice.Domain.Events
{
    public record ConsultationCreeeEvent(Guid ConsultationId, Guid DossierId, DateTimeOffset OccurredAt)
    {
        public ConsultationCreeeEvent(Guid consultationId, Guid dossierId)
            : this(consultationId, dossierId, DateTimeOffset.UtcNow) { }
    }
}
