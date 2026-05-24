using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface ILignePrescriptionRepository : IRepository<LignePrescription>
    {
        Task<IEnumerable<LignePrescription>> GetByPrescriptionAsync(Guid prescriptionId, CancellationToken ct = default);
    }
}
