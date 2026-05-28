namespace Document_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un document est archivé
    // ═══════════════════════════════════════════════════════════
    public record DocumentArchiveEvent(
        Guid DocumentId,
        Guid PatientId,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
