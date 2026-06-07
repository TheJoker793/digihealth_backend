using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface IRapportRepository : IRepository<RapportStatistique>
    {
        Task<IEnumerable<RapportStatistique>> GetByCabinetAsync(
            Guid cabinetId,
            TypeRapport? type = null,
            CancellationToken ct = default);

        Task<IEnumerable<RapportStatistique>> GetPlanifiesAsync(
            CancellationToken ct = default);

        Task<bool> NumeroExisteAsync(string numero, CancellationToken ct = default);
    }
}
