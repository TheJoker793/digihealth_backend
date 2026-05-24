
namespace Prescription_microservice.Domain.Interfaces
{
    public interface IPrescriptionCacheService
    {
        Task<string?> GetActivesAsync(Guid patientId, CancellationToken ct = default);
        Task SetActivesAsync(Guid patientId, string json, TimeSpan ttl, CancellationToken ct = default);
        Task InvalidateAsync(Guid patientId, CancellationToken ct = default);
    }

}
