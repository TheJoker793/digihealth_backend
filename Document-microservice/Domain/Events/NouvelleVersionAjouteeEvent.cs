namespace Document_microservice.Domain.Events
{
    // ═══════════════════════════════════════════════════════════
    // Publié quand une nouvelle version est ajoutée
    // ═══════════════════════════════════════════════════════════
    public record NouvelleVersionAjouteeEvent(
        Guid DocumentId,
        Guid VersionId,
        int NumeroVersion,
        DateTimeOffset OccurredAt = default)
    {
        public DateTimeOffset OccurredAt { get; } =
            OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
    }
}
