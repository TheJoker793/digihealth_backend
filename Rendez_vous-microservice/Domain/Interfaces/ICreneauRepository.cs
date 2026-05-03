using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface ICreneauRepository:IRepository<Creneau>
    {
        Task<IEnumerable<Creneau>> GetDisponiblesAsync(Guid medecinId, DateTime debut, DateTime fin);
      
        Task<bool> ExisteChevaucheAsync(Guid medecinId, DateTime debut, DateTime fin);
    }

}
