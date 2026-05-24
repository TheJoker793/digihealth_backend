using Prescription_microservice.Domain.Interfaces;
using StackExchange.Redis;

namespace Prescription_microservice.Infrastructure.Services
{
    public class NumeroPrescriptionGenerator : INumeroPrescriptionGenerator
    {
        private readonly IDatabase _redis;

        public NumeroPrescriptionGenerator(IConnectionMultiplexer connection)
        {
            _redis = connection.GetDatabase();
        }

        public async Task<string> GenerateAsync(Guid cabinetId, CancellationToken ct = default)
        {
            if (cabinetId == Guid.Empty)
                throw new ArgumentException("Le cabinetId est obligatoire.", nameof(cabinetId));

            var year = DateTime.UtcNow.Year;

            // Clé Redis : prescriptions:{cabinetId}:{year}:seq
            var key = $"prescriptions:{cabinetId}:{year}:seq";

            // Incrément atomique
            var seq = await _redis.StringIncrementAsync(key);

            // Format RX-{cabinetId:6}-{YYYY}-{seq:0000}
            var cab = cabinetId.ToString("N")[..6].ToUpperInvariant(); // 6 premiers caractères du GUID
            var numero = $"RX-{cab}-{year}-{seq:D4}";

            return numero;
        }
    }
}
