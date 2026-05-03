using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Domain.Interfaces
{
    public interface IAntecedentRepository:IRepository<Antecedent>
    {
        Task<IEnumerable<Antecedent>> GetAntecedentsByPatientAsync(Guid patientId);

    }
}
