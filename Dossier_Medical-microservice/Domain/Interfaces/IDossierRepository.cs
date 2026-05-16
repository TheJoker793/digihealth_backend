using Dossier_Medical_microservice.Domain.Entities;

namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IDossierRepository:IRepository<DossierMedical>
    {
        Task<DossierMedical?> GetByNumeroDossierAsync(string numero, CancellationToken ct = default);
        Task<IEnumerable<DossierMedical>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
        Task<bool> ExistsForPatientAsync(Guid patientId, Guid medecinId, CancellationToken ct = default);
    }
}
