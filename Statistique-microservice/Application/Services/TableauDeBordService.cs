using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.DTOs.Responses;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Exceptions;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Application.Services
{
        public class TableauDeBordService
        {
            private readonly IUnitOfWork _uow;
            private readonly KPIService _kpiService;

            public TableauDeBordService(IUnitOfWork uow, KPIService kpiService)
            {
                _uow = uow;
                _kpiService = kpiService;
            }

            public async Task<TableauDeBordResponse> CreerAsync(
                CreerTableauDeBordRequest request,
                CancellationToken ct = default)
            {
                var tableau = TableauDeBord.Create(
                    request.CabinetId,
                    request.ProprietaireId,
                    request.Nom,
                    request.PeriodeDefaut,
                    request.EstParDefaut);

                if (request.KPIsAffiches?.Length > 0)
                {
                    tableau.Personnaliser(
                        request.Nom,
                        request.KPIsAffiches,
                        request.PeriodeDefaut,
                        12);
                }

                await _uow.Tableaux.AddAsync(tableau, ct);
                await _uow.SaveChangesAsync(ct);
                return ToResponse(tableau);
            }

            public async Task<TableauDeBordResponse> PersonnaliserAsync(
                Guid tableauId,
                PersonnaliserTableauRequest request,
                CancellationToken ct = default)
            {
                var tableau = await _uow.Tableaux.GetByIdAsync(tableauId, ct)
                    ?? throw new TableauDeBordIntrouvableException(tableauId);

                tableau.Personnaliser(
                    request.Nom,
                    request.KPIsAffiches,
                    request.PeriodeDefaut,
                    request.NbSemainesTendance);

                _uow.Tableaux.Update(tableau);
                await _uow.SaveChangesAsync(ct);
                return ToResponse(tableau);
            }

            /// <summary>
            /// Retourne le tableau de bord enrichi avec les KPIs calculés
            /// et les snapshots des N dernières semaines (tendance).
            /// </summary>
            public async Task<DashboardResponse> GetDashboardAsync(
                Guid tableauId,
                CancellationToken ct = default)
            {
                var tableau = await _uow.Tableaux.GetByIdAsync(tableauId, ct)
                    ?? throw new TableauDeBordIntrouvableException(tableauId);

                // Calculer les KPIs pour la période par défaut du tableau
                var kpis = await _kpiService.GetKPIsAsync(
                    new GetKPIsRequest(
                        tableau.CabinetId,
                        tableau.PeriodeDefaut,
                        Types: tableau.KPIsAffiches),
                    ct);

                // Récupérer les snapshots pour la fenêtre tendance
                var dateFin = Domain.ValueObjects.PeriodeAnalyse
                    .SemaineCourante().DateFin;
                var dateDebut = dateFin.AddDays(-tableau.NbSemainesTendance * 7);

                var tendance = await _uow.Snapshots
                    .GetPlageDateAsync(tableau.CabinetId, dateDebut, dateFin, ct);

                var snapshotsDto = tendance
                    .OrderBy(s => s.DateSnapshot)
                    .Select(s => new SnapshotResponse(
                        s.Id, s.CabinetId,
                        s.DateSnapshot.ToString("yyyy-MM-dd"),
                        s.NbConsultations, s.NbNouveauxPatients, s.NbPatientsUniques,
                        s.NbRdvConfirmes, s.NbRdvAnnules, s.NbOrdonnances,
                        s.ChiffreAffaires, s.TauxOccupation,
                        s.TicketMoyen(), s.TauxAnnulationRdv(),
                        s.EstConsolide, s.CreatedAt));

                return new DashboardResponse(
                    ToResponse(tableau),
                    kpis,
                    snapshotsDto,
                    DateTimeOffset.UtcNow);
            }

            public async Task<IEnumerable<TableauDeBordResponse>> GetByProprietaireAsync(
                Guid proprietaireId, CancellationToken ct = default)
            {
                var tableaux = await _uow.Tableaux.GetByProprietaireAsync(proprietaireId, ct);
                return tableaux.Select(ToResponse);
            }

            public async Task SupprimerAsync(Guid tableauId, CancellationToken ct = default)
            {
                var tableau = await _uow.Tableaux.GetByIdAsync(tableauId, ct)
                    ?? throw new TableauDeBordIntrouvableException(tableauId);

                _uow.Tableaux.Remove(tableau);
                await _uow.SaveChangesAsync(ct);
            }

        private static TableauDeBordResponse ToResponse(TableauDeBord t)
        => new(t.Id, t.CabinetId, t.ProprietaireId, t.Nom,
               t.KPIsAffiches, t.PeriodeDefaut, t.NbSemainesTendance,
               t.EstParDefaut, t.CreatedAt);

    }
    }
