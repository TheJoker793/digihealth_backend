using Document_microservice.Domain.Enums;

namespace Document_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un document est créé
    // ═══════════════════════════════════════════════════════════
    public record DocumentCreeEvent(
        Guid DocumentId,
        Guid PatientId,
        Guid MedecinId,
        TypeDocument TypeDocument,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
