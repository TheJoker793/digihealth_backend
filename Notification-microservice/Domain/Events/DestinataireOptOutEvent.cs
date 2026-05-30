using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un destinataire fait opt-out
    // ═══════════════════════════════════════════════════════════
    public record DestinataireOptOutEvent(
        Guid DestinataireId,
        string TypeDestinataire,
        CanalEnvoi? Canal,          // null = opt-out global
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
