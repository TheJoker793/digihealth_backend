using StackExchange.Redis;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public class RedisRapportCache : IRapportCache
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisRapportCache(IConnectionMultiplexer redis) => _redis = redis;

        private static string Cle(Guid cabinetId, PeriodeAnalyse p)
            => $"kpi:{cabinetId}:{p.TypePeriode}:{p.DateDebut:yyyyMMdd}:{p.DateFin:yyyyMMdd}";

        public async Task<IEnumerable<IndicateurKPI>?> GetKPIsAsync(
            Guid cabinetId, PeriodeAnalyse periode)
        {
            try
            {
                var db = _redis.GetDatabase();
                var json = await db.StringGetAsync(Cle(cabinetId, periode));
                if (json.IsNullOrEmpty) return null;
                return System.Text.Json.JsonSerializer
                    .Deserialize<IEnumerable<IndicateurKPI>>(json!);
            }
            catch { return null; }
        }

        public async Task SetKPIsAsync(
            Guid cabinetId,
            PeriodeAnalyse periode,
            IEnumerable<IndicateurKPI> kpis,
            TimeSpan expiration)
        {
            try
            {
                var db = _redis.GetDatabase();
                var json = System.Text.Json.JsonSerializer.Serialize(kpis);
                await db.StringSetAsync(Cle(cabinetId, periode), json, expiration);
            }
            catch { /* cache non critique */ }
        }

        public async Task InvaliderAsync(Guid cabinetId)
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: $"kpi:{cabinetId}:*");
                var db = _redis.GetDatabase();
                foreach (var key in keys)
                    await db.KeyDeleteAsync(key);
            }
            catch { /* cache non critique */ }
        }
    }
}
