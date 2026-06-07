using Statistique_microservice.Application.DTOs.Responses;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Application.Services
{
    
        public class SnapshotService
        {
            private readonly IUnitOfWork _uow;
            private readonly IEventPublisher _publisher;

            public SnapshotService(IUnitOfWork uow, IEventPublisher publisher)
            {
                _uow = uow;
                _publisher = publisher;
            }

            // ── Obtenir ou créer le snapshot du jour ─────────────────
            public async Task<Domain.Entities.SnapshotActivite> ObtenirOuCreerAsync(
                Guid cabinetId,
                DateOnly date,
                CancellationToken ct = default)
            {
                var snapshot = await _uow.Snapshots.GetByDateAsync(cabinetId, date, ct);

                if (snapshot is null)
                {
                    snapshot = Domain.Entities.SnapshotActivite.Creer(cabinetId, date);
                    await _uow.Snapshots.AddAsync(snapshot, ct);
                    await _uow.SaveChangesAsync(ct);
                }

                return snapshot;
            }

            // ── Consolider le snapshot du jour (appelé par Hangfire) ─
            public async Task ConsoliderAsync(
                Guid cabinetId,
                DateOnly date,
                int creneauxDisponibles,
                CancellationToken ct = default)
            {
                var snapshot = await _uow.Snapshots.GetByDateAsync(cabinetId, date, ct)
                    ?? Domain.Entities.SnapshotActivite.Creer(cabinetId, date);

                if (snapshot.EstConsolide)
                    return; // déjà consolidé — idempotent

                snapshot.Consolider(creneauxDisponibles);
                _uow.Snapshots.Update(snapshot);
                await _uow.SaveChangesAsync(ct);

                foreach (var evt in snapshot.DomainEvents)
                    await _publisher.PublishAsync(evt, ct);
                snapshot.ClearDomainEvents();
            }

            // ── Consolider tous les cabinets (job quotidien) ─────────
            public async Task ConsoliderTousCabinetsAsync(
                DateOnly date,
                CancellationToken ct = default)
            {
                var cabinetIds = await _uow.Snapshots
                    .GetCabinetsAvecSnapshotNonConsolideAsync(date, ct);

                foreach (var cabinetId in cabinetIds)
                    await ConsoliderAsync(cabinetId, date, creneauxDisponibles: 16, ct);
            }

            // ── Historique des snapshots ─────────────────────────────
            public async Task<IEnumerable<SnapshotResponse>> GetHistoriqueAsync(
                Guid cabinetId,
                DateOnly dateDebut,
                DateOnly dateFin,
                CancellationToken ct = default)
            {
                var snapshots = await _uow.Snapshots
                    .GetPlageDateAsync(cabinetId, dateDebut, dateFin, ct);

                return snapshots
                    .OrderBy(s => s.DateSnapshot)
                    .Select(ToResponse);
            }

            private static SnapshotResponse ToResponse(Domain.Entities.SnapshotActivite s)
                => new(s.Id, s.CabinetId,
                       s.DateSnapshot.ToString("yyyy-MM-dd"),
                       s.NbConsultations, s.NbNouveauxPatients, s.NbPatientsUniques,
                       s.NbRdvConfirmes, s.NbRdvAnnules, s.NbOrdonnances,
                       s.ChiffreAffaires, s.TauxOccupation,
                       s.TicketMoyen(), s.TauxAnnulationRdv(),
                       s.EstConsolide, s.CreatedAt);
        }
    
}
