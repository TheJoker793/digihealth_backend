namespace Notification_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // CanalEnvoi — moyen de communication
    // ═══════════════════════════════════════════════════════════
    public enum CanalEnvoi
    {
        Email = 1,
        SMS = 2,
        Push = 3,   // notification push mobile (Firebase FCM)
        InApp = 4,   // notification in-app (SignalR)
    }
}
