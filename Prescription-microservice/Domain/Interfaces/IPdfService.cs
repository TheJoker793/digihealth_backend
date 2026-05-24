using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> GenererPrescriptionAsync(Prescription prescription, CancellationToken ct = default);
    }
}
