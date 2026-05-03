using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRdvCacheService
    {
        Task<IEnumerable<RendezVous>?> GetAgendaCacheAsync(Guid medecinId, DateTime debut, DateTime fin);
        Task SetAgendaCacheAsync(Guid medecinId, DateTime debut, DateTime fin, IEnumerable<RendezVous> agenda);
        Task InvalidateAsync(Guid medecinId);
    }
}
