using Facturation_microservice.Domain.Interfaces;
using StackExchange.Redis;
using System.Text.Json;


namespace Facturation_microservice.Infrastructure.Services
{
    public class RedisFactureCacheService : IFactureCacheService
    {
        private readonly IDatabase _db;
        public RedisFactureCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<object?> GetStatsAsync(string key)
        {
            var data = await _db.StringGetAsync(key);

            return data.IsNull ? null : JsonSerializer.Deserialize<object>(data!);
        }

        public async Task SetStatsAsync(string key, object data)
        {
            var json = JsonSerializer.Serialize(data);

            await _db.StringSetAsync(key, json, TimeSpan.FromMinutes(5));
        }

        public async Task InvalidateAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }



    }
}
