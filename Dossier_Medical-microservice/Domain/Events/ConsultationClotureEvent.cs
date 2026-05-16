namespace Dossier_Medical_microservice.Domain.Events
{
    public record ConsultationClotureEvent(Guid ConsultationId, Guid DossierId, DateTimeOffset OccurredAt)
    {
        public ConsultationClotureEvent(Guid consultationId, Guid dossierId)
            : this(consultationId, dossierId, DateTimeOffset.UtcNow) { }
    }
}
