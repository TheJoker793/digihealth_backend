using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public class RapportRepository : Repository<RapportStatistique>, IRapportRepository
    {
        public RapportRepository(StatistiqueDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<RapportStatistique>> GetByCabinetAsync(
            Guid cabinetId, TypeRapport? type = null, CancellationToken ct = default)
        {
            var q = _dbSet.AsNoTracking()
                .Where(r => r.CabinetId == cabinetId);
            if (type.HasValue) q = q.Where(r => r.TypeRapport == type.Value);
            return await q.OrderByDescending(r => r.CreatedAt).ToListAsync(ct);
        }

        public async Task<IEnumerable<RapportStatistique>> GetPlanifiesAsync(
            CancellationToken ct = default)
            => await _dbSet
                .Where(r => r.Statut == StatutRapport.Planifie
                         && r.DatePlanifiee <= DateTimeOffset.UtcNow)
                .ToListAsync(ct);

        public async Task<bool> NumeroExisteAsync(string numero, CancellationToken ct = default)
            => await _dbSet.AnyAsync(r => r.Numero == numero, ct);
    }
}
