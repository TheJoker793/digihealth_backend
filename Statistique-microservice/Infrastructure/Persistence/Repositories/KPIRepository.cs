using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public class KPIRepository : Repository<IndicateurKPI>, IKPIRepository
    {
        public KPIRepository(StatistiqueDbContext ctx) : base(ctx) { }

        public async Task<IndicateurKPI?> GetByTypeEtPeriodeAsync(
            Guid cabinetId, TypeKPI typeKPI, PeriodeAnalyse periode, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(k =>
                    k.CabinetId == cabinetId
                 && k.TypeKPI == typeKPI
                 && k.Periode.DateDebut == periode.DateDebut
                 && k.Periode.DateFin == periode.DateFin, ct);

        public async Task<IEnumerable<IndicateurKPI>> GetHistoriqueAsync(
            Guid cabinetId, TypeKPI typeKPI, int nbPeriodes, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(k => k.CabinetId == cabinetId && k.TypeKPI == typeKPI)
                .OrderByDescending(k => k.Periode.DateDebut)
                .Take(nbPeriodes)
                .ToListAsync(ct);

        public async Task<IEnumerable<IndicateurKPI>> GetByCabinetEtPeriodeAsync(
            Guid cabinetId, PeriodeAnalyse periode, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(k => k.CabinetId == cabinetId
                         && k.Periode.DateDebut == periode.DateDebut
                         && k.Periode.DateFin == periode.DateFin)
                .ToListAsync(ct);
    }
}
