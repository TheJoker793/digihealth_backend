using Facturation_microservice.Domain.Interfaces;
using StackExchange.Redis;

namespace Facturation_microservice.Infrastructure.Services
{
    public class NumeroFactureGenerator : INumeroFactureGenerator
    {
        private readonly IDatabase _db;
        public NumeroFactureGenerator(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<string> GenerateAsync(Guid cabinetId)
        {
            var year = DateTime.UtcNow.Year;

            var key = $"facture:{cabinetId}:{year}";

            var seq = await _db.StringIncrementAsync(key);

            return $"FACT-{cabinetId.ToString()[..6].ToUpper()}-{year}-{seq:D4}";
        }
    }
}
