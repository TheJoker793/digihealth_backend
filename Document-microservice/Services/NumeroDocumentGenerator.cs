using Document_microservice.Domain.Interfaces;

namespace Document_microservice.Services
{
    public class NumeroDocumentGenerator : INumeroDocumentGenerator
    {
        private static readonly object _lock = new();

        public Task<string> GenererAsync(Guid cabinetId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            lock (_lock)
            {
                // Format exemple : CAB-{cabinet}-{yyyyMMdd}-{sequence}
                var datePart = DateTime.UtcNow.ToString("yyyyMMdd");

                // Simulation d’une séquence (à remplacer par DB plus tard)
                var sequence = new Random().Next(1, 99999);

                var numero = $"DOC-{cabinetId.ToString().Substring(0, 8)}-{datePart}-{sequence:00000}";

                return Task.FromResult(numero);
            }
        }
    }
}