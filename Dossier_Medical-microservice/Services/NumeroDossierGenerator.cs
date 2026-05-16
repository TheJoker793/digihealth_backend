using Dossier_Medical_microservice.Domain.Interfaces;
using StackExchange.Redis;

namespace Dossier_Medical_microservice.Services
{
    public class NumeroDossierGenerator : INumeroDossierGenerator
    {
        private readonly IDatabase _redis;

        public NumeroDossierGenerator(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
        public async Task<string> GenerateAsync(Guid cabinetId, CancellationToken ct = default)
        {
            var year = DateTime.UtcNow.Year;
            var key = $"seq:dossier:{cabinetId}:{year}";

            var seq = await _redis.StringIncrementAsync(key);

            return $"DMI-{cabinetId}-{year}-{seq:0000}";
        }
    }
}
