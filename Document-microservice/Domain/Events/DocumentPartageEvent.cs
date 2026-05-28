namespace Document_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un document est partagé
    // ═══════════════════════════════════════════════════════════
    public record DocumentPartageEvent(
        Guid DocumentId,
        Guid PatientId,
        Guid DestinataireId,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
