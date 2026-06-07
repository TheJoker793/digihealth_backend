using MassTransit;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Application.Consumers
{
    public class PrescriptionSigneeStatConsumer : IConsumer<PrescriptionSigneeStatEvent>
    {
        private readonly SnapshotService _snapshotService;

        public PrescriptionSigneeStatConsumer(SnapshotService snapshotService)
            => _snapshotService = snapshotService;

        public async Task Consume(ConsumeContext<PrescriptionSigneeStatEvent> context)
        {
            var evt = context.Message;
            var date = DateOnly.FromDateTime(evt.Date.UtcDateTime);
            var snapshot = await _snapshotService
                .ObtenirOuCreerAsync(evt.CabinetId, date, context.CancellationToken);

            snapshot.AjouterOrdonnance();
        }
    }
}
