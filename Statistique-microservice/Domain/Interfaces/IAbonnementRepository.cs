using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface IAbonnementRepository : IRepository<AbonnementRapport>
    {
        Task<IEnumerable<AbonnementRapport>> GetEchusAsync(
            CancellationToken ct = default);

        Task<IEnumerable<AbonnementRapport>> GetByCabinetAsync(
            Guid cabinetId,
            CancellationToken ct = default);
    }
}
