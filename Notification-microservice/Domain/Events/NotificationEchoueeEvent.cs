using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié après épuisement des tentatives
    // ═══════════════════════════════════════════════════════════
    public record NotificationEchoueeEvent(
        Guid NotificationId,
        Guid DestinataireId,
        TypeEvenement TypeEvenement,
        CanalEnvoi Canal,
        string DerniereErreur,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }

}
