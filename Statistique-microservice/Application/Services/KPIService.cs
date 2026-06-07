using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.DTOs.Responses;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Application.Services
{
    public class KPIService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICalculateurKPI _calculateur;
        private readonly IRapportCache _cache;

        public KPIService(
            IUnitOfWork uow,
            ICalculateurKPI calculateur,
            IRapportCache cache)
        {
            _uow = uow;
            _calculateur = calculateur;
            _cache = cache;
        }

        // ── Calcule et retourne les KPIs pour une période ────────
        public async Task<IEnumerable<KPIResponse>> GetKPIsAsync(
            GetKPIsRequest request,
            CancellationToken ct = default)
        {
            var periode = ResoudrePeriode(request);

            // Vérifier le cache Redis (TTL 1h)
            var cached = await _cache.GetKPIsAsync(request.CabinetId, periode);
            if (cached is not null)
            {
                var filtres = FiltrerParType(cached, request.Types);
                return filtres.Select(RapportService.ToKPIResponse);
            }

            // Calculer depuis les snapshots
            var kpis = await _calculateur.CalculerAsync(request.CabinetId, periode, ct);

            // Mettre en cache 1 heure
            await _cache.SetKPIsAsync(
                request.CabinetId, periode, kpis, TimeSpan.FromHours(1));

            var resultat = FiltrerParType(kpis, request.Types);
            return resultat.Select(RapportService.ToKPIResponse);
        }

        // ── Historique d'un KPI sur N périodes ──────────────────
        public async Task<IEnumerable<KPIResponse>> GetHistoriqueAsync(
            Guid cabinetId,
            TypeKPI typeKPI,
            int nbPeriodes = 12,
            CancellationToken ct = default)
        {
            var historique = await _uow.KPIs
                .GetHistoriqueAsync(cabinetId, typeKPI, nbPeriodes, ct);

            return historique
                .OrderBy(k => k.Periode.DateDebut)
                .Select(RapportService.ToKPIResponse);
        }

        // ── Helpers ──────────────────────────────────────────────
        private static IEnumerable<Domain.Entities.IndicateurKPI> FiltrerParType(
            IEnumerable<Domain.Entities.IndicateurKPI> kpis,
            TypeKPI[]? types)
            => types is null || types.Length == 0
                ? kpis
                : kpis.Where(k => types.Contains(k.TypeKPI));

        private static PeriodeAnalyse ResoudrePeriode(GetKPIsRequest req)
            => req.TypePeriode switch
            {
                TypePeriode.Journee => PeriodeAnalyse.Journee(
                    DateOnly.FromDateTime(DateTime.UtcNow)),
                TypePeriode.Hebdomadaire => PeriodeAnalyse.SemaineCourante(),
                TypePeriode.Mensuel => PeriodeAnalyse.MoisCourant(),
                TypePeriode.Annuel => PeriodeAnalyse.AnneeCourante(),
                TypePeriode.Personnalise => PeriodeAnalyse.Personnalisee(
                    DateOnly.Parse(req.DateDebut!),
                    DateOnly.Parse(req.DateFin!)),
                _ => PeriodeAnalyse.MoisCourant()
            };
    }
}
