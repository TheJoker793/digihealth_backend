using Notification_microservice.Domain.Interfaces;
using StackExchange.Redis;

namespace Notification_microservice.Infrastructure.Messaging
{
    public class RedisNumeroNotificationGenerator : INumeroNotificationGenerator
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisNumeroNotificationGenerator(IConnectionMultiplexer redis)
            => _redis = redis;

        public async Task<string> GenererAsync(CancellationToken ct = default)
        {
            var db = _redis.GetDatabase();
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var key = $"notif:seq:{date}";

            // INCR atomique — Redis garantit l'unicité
            var seq = await db.StringIncrementAsync(key);

            // TTL de 2 jours pour nettoyage automatique
            await db.KeyExpireAsync(key, TimeSpan.FromDays(2));

            return $"NOTIF-{date}-{seq:D4}";
        }
    }
}
