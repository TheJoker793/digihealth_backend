using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface IRapportCache
    {
        Task<IEnumerable<IndicateurKPI>?> GetKPIsAsync(
            Guid cabinetId, PeriodeAnalyse periode);

        Task SetKPIsAsync(
            Guid cabinetId,
            PeriodeAnalyse periode,
            IEnumerable<IndicateurKPI> kpis,
            TimeSpan expiration);

        Task InvaliderAsync(Guid cabinetId);
    }
}
