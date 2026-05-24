using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface INumeroPrescriptionGenerator
    {
        Task<string> GenerateAsync(Guid cabinetId, CancellationToken ct = default);
    }
}
