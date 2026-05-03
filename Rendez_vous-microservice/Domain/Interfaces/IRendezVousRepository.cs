using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRendezVousRepository:IRepository<RendezVous>
    {
        Task<IEnumerable<RendezVous>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<RendezVous>> GetByMedecinAsync(Guid medecinId);
        Task<IEnumerable<RendezVous>> GetAgendaAsync(Guid medecinId, DateTime debut, DateTime fin);

       
    }
}
