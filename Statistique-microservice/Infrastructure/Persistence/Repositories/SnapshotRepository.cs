using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public class SnapshotRepository : Repository<SnapshotActivite>, ISnapshotRepository
    {
        public SnapshotRepository(StatistiqueDbContext ctx) : base(ctx) { }

        public async Task<SnapshotActivite?> GetByDateAsync(
            Guid cabinetId, DateOnly date, CancellationToken ct = default)
            => await _dbSet
                .FirstOrDefaultAsync(
                    s => s.CabinetId == cabinetId && s.DateSnapshot == date, ct);

        public async Task<IEnumerable<SnapshotActivite>> GetPlageDateAsync(
            Guid cabinetId, DateOnly debut, DateOnly fin, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(s => s.CabinetId == cabinetId
                         && s.DateSnapshot >= debut
                         && s.DateSnapshot <= fin)
                .OrderBy(s => s.DateSnapshot)
                .ToListAsync(ct);

        public async Task<SnapshotActivite?> GetDernierConsolideAsync(
            Guid cabinetId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(s => s.CabinetId == cabinetId && s.EstConsolide)
                .OrderByDescending(s => s.DateSnapshot)
                .FirstOrDefaultAsync(ct);

        public async Task<IEnumerable<Guid>> GetCabinetsAvecSnapshotNonConsolideAsync(
            DateOnly date, CancellationToken ct = default)
        {
            var ids = await _dbSet
                .Where(s => s.DateSnapshot == date && !s.EstConsolide)
                .Select(s => s.CabinetId)
                .Distinct()
                .ToListAsync(ct);
            return ids;
        }
    }
}
