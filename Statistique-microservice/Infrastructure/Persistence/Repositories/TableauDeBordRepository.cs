using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public class TableauDeBordRepository
    : Repository<TableauDeBord>, ITableauDeBordRepository
    {
        public TableauDeBordRepository(StatistiqueDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<TableauDeBord>> GetByProprietaireAsync(
            Guid proprietaireId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(t => t.ProprietaireId == proprietaireId)
                .OrderByDescending(t => t.EstParDefaut)
                .ThenBy(t => t.Nom)
                .ToListAsync(ct);

        public async Task<TableauDeBord?> GetParDefautAsync(
            Guid proprietaireId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.ProprietaireId == proprietaireId && t.EstParDefaut, ct);
    }
}
