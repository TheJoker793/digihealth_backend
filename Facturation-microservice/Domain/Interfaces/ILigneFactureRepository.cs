using Facturation_microservice.Domain.Entities;

namespace Facturation_microservice.Domain.Interfaces
{
    public interface ILigneFactureRepository : IRepository<LigneFacture>
    {
        Task<IEnumerable<LigneFacture>> GetByFactureAsync(Guid factureId);

    }
}
