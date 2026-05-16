using Dossier_Medical_microservice.Domain.Entities;

namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IOrdonnanceRepository: IRepository<Ordonnance>
    {
        Task<IEnumerable<Ordonnance>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default);
        Task<IEnumerable<Ordonnance>> GetActiveByPatientAsync(Guid patientId, CancellationToken ct = default);
    }
}
