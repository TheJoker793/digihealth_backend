using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<Prescription?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Prescription>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
        Task<IEnumerable<Prescription>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default);
        Task<IEnumerable<Prescription>> GetActivesAsync(Guid patientId, CancellationToken ct = default);
        Task<IEnumerable<Prescription>> GetExpireesAsync(CancellationToken ct = default);
        Task<Prescription?> GetByNumeroAsync(string numero, CancellationToken ct = default);
    }

}
