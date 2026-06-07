using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Infrastructure.Calcul
{
    public class CalculateurKPIService : ICalculateurKPI
    {
        private readonly ISnapshotRepository _snapshotRepo;
        private readonly IKPIRepository _kpiRepo;

        public CalculateurKPIService(
            ISnapshotRepository snapshotRepo,
            IKPIRepository kpiRepo)
        {
            _snapshotRepo = snapshotRepo;
            _kpiRepo = kpiRepo;
        }

        public async Task<IEnumerable<IndicateurKPI>> CalculerAsync(
            Guid cabinetId,
            PeriodeAnalyse periode,
            CancellationToken ct = default)
        {
            // 1. Charger snapshots de la période courante
            var snapshots = (await _snapshotRepo.GetPlageDateAsync(
                cabinetId, periode.DateDebut, periode.DateFin, ct)).ToList();

            // 2. Charger snapshots de la période précédente (pour variation N-1)
            var periodePrecedente = periode.PeriodePrecedente();
            var snapshotsPrecedents = (await _snapshotRepo.GetPlageDateAsync(
                cabinetId,
                periodePrecedente.DateDebut,
                periodePrecedente.DateFin, ct)).ToList();

            // 3. Calculer chaque KPI
            var kpis = new List<IndicateurKPI>();

            kpis.Add(CreerKPI(cabinetId, TypeKPI.NbConsultations,
                "NB_CONSULTATIONS", "consultations", periode,
                snapshots.Sum(s => s.NbConsultations),
                snapshotsPrecedents.Sum(s => s.NbConsultations)));

            kpis.Add(CreerKPI(cabinetId, TypeKPI.NbNouveauxPatients,
                "NB_NOUVEAUX_PATIENTS", "patients", periode,
                snapshots.Sum(s => s.NbNouveauxPatients),
                snapshotsPrecedents.Sum(s => s.NbNouveauxPatients)));

            kpis.Add(CreerKPI(cabinetId, TypeKPI.NbPatientsUniques,
                "NB_PATIENTS_UNIQUES", "patients", periode,
                snapshots.Sum(s => s.NbPatientsUniques),
                snapshotsPrecedents.Sum(s => s.NbPatientsUniques)));

            kpis.Add(CreerKPI(cabinetId, TypeKPI.NbRdvConfirmes,
                "NB_RDV_CONFIRMES", "RDV", periode,
                snapshots.Sum(s => s.NbRdvConfirmes),
                snapshotsPrecedents.Sum(s => s.NbRdvConfirmes)));

            kpis.Add(CreerKPI(cabinetId, TypeKPI.NbOrdonnances,
                "NB_ORDONNANCES", "ordonnances", periode,
                snapshots.Sum(s => s.NbOrdonnances),
                snapshotsPrecedents.Sum(s => s.NbOrdonnances)));

            kpis.Add(CreerKPI(cabinetId, TypeKPI.ChiffreAffaires,
                "CHIFFRE_AFFAIRES", "TND", periode,
                snapshots.Sum(s => s.ChiffreAffaires),
                snapshotsPrecedents.Sum(s => s.ChiffreAffaires)));

            // Taux d'occupation = moyenne des taux journaliers (évite la division par zéro)
            var tauxOccup = snapshots.Any(s => s.TauxOccupation > 0)
                ? snapshots.Where(s => s.TauxOccupation > 0).Average(s => s.TauxOccupation)
                : 0m;
            var tauxOccupPrec = snapshotsPrecedents.Any(s => s.TauxOccupation > 0)
                ? snapshotsPrecedents.Where(s => s.TauxOccupation > 0).Average(s => s.TauxOccupation)
                : 0m;

            kpis.Add(CreerKPI(cabinetId, TypeKPI.TauxOccupation,
                "TAUX_OCCUPATION", "%", periode,
                Math.Round(tauxOccup, 2),
                Math.Round(tauxOccupPrec, 2)));

            // Ticket moyen
            var totalFactures = snapshots.Sum(s => s.NbFactures);
            var totalCA = snapshots.Sum(s => s.ChiffreAffaires);
            var ticketMoyen = totalFactures > 0 ? totalCA / totalFactures : 0m;

            var totalFacturesPrec = snapshotsPrecedents.Sum(s => s.NbFactures);
            var totalCAPrec = snapshotsPrecedents.Sum(s => s.ChiffreAffaires);
            var ticketMoyenPrec = totalFacturesPrec > 0 ? totalCAPrec / totalFacturesPrec : 0m;

            kpis.Add(CreerKPI(cabinetId, TypeKPI.TicketMoyenConsultation,
                "TICKET_MOYEN", "TND", periode,
                Math.Round(ticketMoyen, 3),
                Math.Round(ticketMoyenPrec, 3)));

            // Taux annulation RDV
            var totalRdv = snapshots.Sum(s => s.NbRdvConfirmes);
            var totalAnnules = snapshots.Sum(s => s.NbRdvAnnules);
            var tauxAnn = totalRdv > 0
                ? Math.Round((decimal)totalAnnules / totalRdv * 100m, 2) : 0m;

            var totalRdvPrec = snapshotsPrecedents.Sum(s => s.NbRdvConfirmes);
            var totalAnnulesPrec = snapshotsPrecedents.Sum(s => s.NbRdvAnnules);
            var tauxAnnPrec = totalRdvPrec > 0
                ? Math.Round((decimal)totalAnnulesPrec / totalRdvPrec * 100m, 2) : 0m;

            kpis.Add(CreerKPI(cabinetId, TypeKPI.TauxAnnulationRdv,
                "TAUX_ANNULATION_RDV", "%", periode, tauxAnn, tauxAnnPrec));

            return kpis;
        }

        public async Task<IndicateurKPI?> CalculerUnAsync(
            Guid cabinetId,
            TypeKPI typeKPI,
            PeriodeAnalyse periode,
            CancellationToken ct = default)
        {
            var tous = await CalculerAsync(cabinetId, periode, ct);
            return tous.FirstOrDefault(k => k.TypeKPI == typeKPI);
        }

        // ── Helper ──────────────────────────────────────────────
        private static IndicateurKPI CreerKPI(
            Guid cabinetId, TypeKPI type, string code,
            string unite, PeriodeAnalyse periode,
            decimal valeur, decimal valeurPrecedente)
            => IndicateurKPI.Create(
                cabinetId, type, code,
                valeur, unite, periode,
                valeurPrecedente: valeurPrecedente == 0 ? null : valeurPrecedente);
    }

}
