namespace Facturation_microservice.Domain.Interfaces
{
    public interface IFactureCacheService
    {
        Task<object?> GetStatsAsync(string key);

        Task SetStatsAsync(string key, object data);

        Task InvalidateAsync(string key);
    }
}
