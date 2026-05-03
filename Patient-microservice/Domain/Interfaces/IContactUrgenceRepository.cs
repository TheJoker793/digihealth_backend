using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Domain.Interfaces
{
    public interface IContactUrgenceRepository:IRepository<ContactUrgence>
    {
        Task<IEnumerable<ContactUrgence>> GetByPatient(Guid patientId);
    }
}
