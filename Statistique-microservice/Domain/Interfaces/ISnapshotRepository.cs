using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface ISnapshotRepository : IRepository<SnapshotActivite>
    {
        Task<SnapshotActivite?> GetByDateAsync(
            Guid cabinetId,
            DateOnly date,
            CancellationToken ct = default);

        Task<IEnumerable<SnapshotActivite>> GetPlageDateAsync(
            Guid cabinetId,
            DateOnly dateDebut,
            DateOnly dateFin,
            CancellationToken ct = default);

        Task<SnapshotActivite?> GetDernierConsolideAsync(
            Guid cabinetId,
            CancellationToken ct = default);

        Task<IEnumerable<Guid>> GetCabinetsAvecSnapshotNonConsolideAsync(
            DateOnly date,
            CancellationToken ct = default);
    }

}
