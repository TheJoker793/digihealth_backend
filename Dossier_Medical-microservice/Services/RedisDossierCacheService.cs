using Dossier_Medical_microservice.Domain.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Dossier_Medical_microservice.Services
{
    public class RedisDossierCacheService : IDossierCacheService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(10);

        public RedisDossierCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetAsync(Guid dossierId, object resume, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(resume);
            await _cache.StringSetAsync($"dossier:{dossierId}", json, _ttl);
        }

        public async Task<T?> GetAsync<T>(Guid dossierId, CancellationToken ct = default)
        {
            var value = await _cache.StringGetAsync($"dossier:{dossierId}");
            return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
        }

        public async Task InvalidateAsync(Guid dossierId, CancellationToken ct = default)
        {
            await _cache.KeyDeleteAsync($"dossier:{dossierId}");
        }

        // 🔹 Retourne le résumé brut (JSON) si présent
        public async Task<string?> GetResumeAsync(Guid dossierId, CancellationToken ct = default)
        {
            var value = await _cache.StringGetAsync($"resume:{dossierId}");
            return value.HasValue ? value.ToString() : null;
        }

        // 🔹 Stocke un résumé brut (JSON) avec TTL custom
        public async Task SetResumeAsync(Guid dossierId, string json, TimeSpan ttl, CancellationToken ct = default)
        {
            await _cache.StringSetAsync($"resume:{dossierId}", json, ttl);
        }
    }
}
