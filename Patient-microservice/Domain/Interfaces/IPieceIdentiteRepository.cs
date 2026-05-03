using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Domain.Interfaces
{
    public interface IPieceIdentiteRepository:IRepository<PieceIdentite>
    {
        Task<IEnumerable<PieceIdentite>> GetByPatient(Guid patientId);
    }
}
