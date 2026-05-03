using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRegleRecurrenceRepository:IRepository<RegleRecurrence>
    {
        Task<IEnumerable<RegleRecurrence>> GetByMedecinAsync(Guid medecinId);
    }
}
