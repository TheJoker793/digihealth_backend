using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié après un envoi réussi
    // ═══════════════════════════════════════════════════════════
    public record NotificationEnvoyeeEvent(
        Guid NotificationId,
        Guid DestinataireId,
        TypeEvenement TypeEvenement,
        CanalEnvoi Canal,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }

}
