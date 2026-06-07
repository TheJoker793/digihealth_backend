using StackExchange.Redis;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public class RedisNumeroRapportGenerator : INumeroRapportGenerator
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisNumeroRapportGenerator(IConnectionMultiplexer redis)
            => _redis = redis;

        public async Task<string> GenererAsync(
            Guid cabinetId, Domain.Enums.TypeRapport type, CancellationToken ct = default)
        {
            var db = _redis.GetDatabase();
            var annee = DateTime.UtcNow.Year;
            var prefixe = type.ToString()[..3].ToUpper();
            var key = $"rapport:seq:{cabinetId}:{annee}:{prefixe}";
            var seq = await db.StringIncrementAsync(key);
            await db.KeyExpireAsync(key, TimeSpan.FromDays(400));
            return $"RPT-{prefixe}-{annee}-{seq:D4}";
        }
    }
}
