namespace Dossier_Medical_microservice.Domain.Events
{
    public record OrdonnanceCreeeEvent(Guid OrdonnanceId, Guid ConsultationId, Guid DossierId, DateTimeOffset OccurredAt)
    {
        public OrdonnanceCreeeEvent(Guid ordonnanceId, Guid consultationId, Guid dossierId)
            : this(ordonnanceId, consultationId, dossierId, DateTimeOffset.UtcNow) { }
    }
}
