using Document_microservice.Domain.Enums;

namespace Document_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un document passe au statut Publié
    // ═══════════════════════════════════════════════════════════
    public record DocumentPublieEvent(
        Guid DocumentId,
        Guid PatientId,
        TypeDocument TypeDocument,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
