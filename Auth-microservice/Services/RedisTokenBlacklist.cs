using Auth_microservice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Auth_microservice.Services
{
    public class RedisTokenBlacklist : ITokenBlacklist
    {
        private readonly IDatabase _db;

        public RedisTokenBlacklist(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task BlacklistAsync(string jti, TimeSpan ttl)
        {
            await _db.StringSetAsync(jti, "blacklisted", ttl);
        }

        public async Task<bool> IsBlacklistedAsync(string jti)
        {
            return await _db.KeyExistsAsync(jti);
        }
    }
}
