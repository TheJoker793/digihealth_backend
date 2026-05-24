using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface IInteractionRepository : IRepository<InteractionDetectee>
    {
        Task<IEnumerable<InteractionDetectee>> GetByPrescriptionAsync(Guid prescriptionId, CancellationToken ct = default);
        Task<IEnumerable<InteractionDetectee>> GetNonContourneesAsync(Guid prescriptionId, CancellationToken ct = default);
    }
}
