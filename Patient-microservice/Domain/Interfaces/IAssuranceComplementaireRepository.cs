using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Domain.Interfaces
{
    public interface IAssuranceComplementaireRepository:IRepository<AssuranceComplementaire>
    {
        Task<IEnumerable<AssuranceComplementaire>> GetActiveByPatient(Guid patientId);
    }
}
