using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public class AbonnementRepository
    : Repository<AbonnementRapport>, IAbonnementRepository
    {
        public AbonnementRepository(StatistiqueDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<AbonnementRapport>> GetEchusAsync(
            CancellationToken ct = default)
            => await _dbSet
                .Where(a => a.EstActif
                         && a.ProchainEnvoi.HasValue
                         && a.ProchainEnvoi <= DateTimeOffset.UtcNow)
                .ToListAsync(ct);

        public async Task<IEnumerable<AbonnementRapport>> GetByCabinetAsync(
            Guid cabinetId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(a => a.CabinetId == cabinetId)
                .OrderBy(a => a.TypeRapport)
                .ToListAsync(ct);
    }
}
