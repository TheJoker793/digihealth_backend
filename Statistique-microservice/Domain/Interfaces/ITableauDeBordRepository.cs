using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface ITableauDeBordRepository : IRepository<TableauDeBord>
    {
        Task<IEnumerable<TableauDeBord>> GetByProprietaireAsync(
            Guid proprietaireId,
            CancellationToken ct = default);

        Task<TableauDeBord?> GetParDefautAsync(
            Guid proprietaireId,
            CancellationToken ct = default);
    }
}
