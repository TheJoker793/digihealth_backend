using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface ICalculateurKPI
    {
        /// <summary>
        /// Calcule tous les KPIs pour un cabinet sur une période.
        /// Lit les snapshots consolidés en base, calcule la variation N-1.
        /// </summary>
        Task<IEnumerable<IndicateurKPI>> CalculerAsync(
            Guid cabinetId,
            PeriodeAnalyse periode,
            CancellationToken ct = default);

        Task<IndicateurKPI?> CalculerUnAsync(
            Guid cabinetId,
            TypeKPI typeKPI,
            PeriodeAnalyse periode,
            CancellationToken ct = default);
    }
}
