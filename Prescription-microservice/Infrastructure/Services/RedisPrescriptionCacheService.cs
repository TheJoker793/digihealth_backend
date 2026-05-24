using Prescription_microservice.Domain.Interfaces;
using StackExchange.Redis;

namespace Prescription_microservice.Infrastructure.Services
{
    public class RedisPrescriptionCacheService : IPrescriptionCacheService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(5);
        public RedisPrescriptionCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }
        // Récupère les prescriptions actives en cache
        public async Task<string?> GetActivesAsync(Guid patientId, CancellationToken ct = default)
        {
            var value = await _cache.StringGetAsync($"prescriptions:actives:{patientId}");
            return value.HasValue ? value.ToString() : null;
        }

        // Met en cache les prescriptions actives
        public async Task SetActivesAsync(Guid patientId, string json, TimeSpan ttl, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Le contenu JSON est obligatoire.", nameof(json));

            await _cache.StringSetAsync(
                $"prescriptions:actives:{patientId}",
                json,
                ttl == default ? _defaultTtl : ttl
            );
        }

        // Invalide le cache pour un patient
        public async Task InvalidateAsync(Guid patientId, CancellationToken ct = default)
        {
            await _cache.KeyDeleteAsync($"prescriptions:actives:{patientId}");
        }
    }
}
