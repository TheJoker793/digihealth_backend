using Facturation_microservice.Domain.Entities;

namespace Facturation_microservice.Domain.Interfaces
{
    public interface IRemboursementRepository : IRepository<RemboursementCaisse>
    {
        Task<IEnumerable<RemboursementCaisse>> GetByFactureAsync(Guid factureId);

        Task<IEnumerable<RemboursementCaisse>> GetEnAttenteAsync();
    }
}
