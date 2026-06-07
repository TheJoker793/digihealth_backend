using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface IKPIRepository : IRepository<IndicateurKPI>
    {
        Task<IndicateurKPI?> GetByTypeEtPeriodeAsync(
            Guid cabinetId,
            TypeKPI typeKPI,
            PeriodeAnalyse periode,
            CancellationToken ct = default);

        Task<IEnumerable<IndicateurKPI>> GetHistoriqueAsync(
            Guid cabinetId,
            TypeKPI typeKPI,
            int nbPeriodes,
            CancellationToken ct = default);

        Task<IEnumerable<IndicateurKPI>> GetByCabinetEtPeriodeAsync(
            Guid cabinetId,
            PeriodeAnalyse periode,
            CancellationToken ct = default);
    }
}
