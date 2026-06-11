using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand un rapport est généré avec succès
    // → Notification.svc l'envoie aux abonnés par email
    // ═══════════════════════════════════════════════════════════
    public record RapportGenereEvent(
        Guid RapportId,
        Guid CabinetId,
        TypeRapport TypeRapport,
        PeriodeAnalyse Periode,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }

    // ═══════════════════════════════════════════════════════════
    // Publié quand un snapshot quotidien est consolidé
    // → peut déclencher des alertes si KPIs hors seuils
    // ═══════════════════════════════════════════════════════════
    public record SnapshotConsolideEvent(
        Guid SnapshotId,
        Guid CabinetId,
        DateOnly DateSnapshot,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }

}
