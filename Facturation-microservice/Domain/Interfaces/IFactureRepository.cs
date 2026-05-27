using Facturation_microservice.Domain.Entities;

namespace Facturation_microservice.Domain.Interfaces
{
    public interface IFactureRepository : IRepository<Facture>
    {
        Task<IEnumerable<Facture>> GetByPatientAsync(Guid patientId);

        Task<IEnumerable<Facture>> GetByPeriodeAsync(DateOnly debut, DateOnly fin);

        Task<IEnumerable<Facture>> GetEnRetardAsync();

        Task<object> GetStatsAsync();
    }
}
