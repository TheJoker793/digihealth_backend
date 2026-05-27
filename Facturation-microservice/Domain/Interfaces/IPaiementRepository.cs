using Facturation_microservice.Domain.Entities;

namespace Facturation_microservice.Domain.Interfaces
{
    public interface IPaiementRepository : IRepository<Paiement>
    {
        Task<IEnumerable<Paiement>> GetByFactureAsync(Guid factureId);

        Task<IEnumerable<Paiement>> GetByPeriodeAsync(DateOnly debut, DateOnly fin);
    }
}
