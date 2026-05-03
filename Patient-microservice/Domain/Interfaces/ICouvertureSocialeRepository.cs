using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Domain.Interfaces
{
    public interface ICouvertureSocialeRepository:IRepository<CouvertureSociale>
    {
        Task<IEnumerable<CouvertureSociale>> GetActiveByPatient(Guid patientId);
    }
}
