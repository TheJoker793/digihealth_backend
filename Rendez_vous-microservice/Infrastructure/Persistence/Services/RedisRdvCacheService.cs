using StackExchange.Redis;
using Rendez_vous_microservice.Domain.Interfaces;
using System.Text.Json;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Services
{
    public class RedisRdvCacheService : IRdvCacheService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(5);

        public RedisRdvCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetAgendaAsync(Guid medecinId, object agenda)
        {
            var key = $"agenda:{medecinId}";
            var value = JsonSerializer.Serialize(agenda);
            await _cache.StringSetAsync(key, value, _ttl);
        }

        public async Task<object?> GetAgendaAsync(Guid medecinId)
        {
            var key = $"agenda:{medecinId}";
            var value = await _cache.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<object>(value!) : null;
        }

        public async Task InvalidateAgendaAsync(Guid medecinId)
        {
            var key = $"agenda:{medecinId}";
            await _cache.KeyDeleteAsync(key);
        }

        public async Task<IEnumerable<RendezVous>?> GetAgendaCacheAsync(Guid medecinId, DateTime debut, DateTime fin)
        {
            var key = BuildKey(medecinId, debut, fin);
            var value = await _cache.StringGetAsync(key);

            if (!value.HasValue)
                return null;

            return JsonSerializer.Deserialize<IEnumerable<RendezVous>>(value!);
        }

        public async Task SetAgendaCacheAsync(Guid medecinId, DateTime debut, DateTime fin, IEnumerable<RendezVous> agenda)
        {
            var key = BuildKey(medecinId, debut, fin);
            var value = JsonSerializer.Serialize(agenda);

            await _cache.StringSetAsync(key, value, _ttl);
        }

        public async Task InvalidateAsync(Guid medecinId)
        {
            // Invalidation de toutes les clés liées au médecin
            var server = _cache.Multiplexer.GetServer(_cache.Multiplexer.GetEndPoints().First());
            foreach (var key in server.Keys(pattern: $"agenda:{medecinId}:*"))
            {
                await _cache.KeyDeleteAsync(key);
            }
        }

        private static string BuildKey(Guid medecinId, DateTime debut, DateTime fin)
        {
            return $"agenda:{medecinId}:{debut:yyyyMMddHHmm}-{fin:yyyyMMddHHmm}";
        }


    }
}
