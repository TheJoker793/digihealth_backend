namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IDossierCacheService
    {
        Task<string?> GetResumeAsync(Guid dossierId, CancellationToken ct = default);
        Task SetResumeAsync(Guid dossierId, string json, TimeSpan ttl, CancellationToken ct = default);
        Task InvalidateAsync(Guid dossierId, CancellationToken ct = default);
    }
}
