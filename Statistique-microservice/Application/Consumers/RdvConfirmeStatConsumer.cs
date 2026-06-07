using MassTransit;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Application.Consumers
{
    public class RdvConfirmeStatConsumer : IConsumer<RdvConfirmeStatEvent>
    {
        private readonly SnapshotService _snapshotService;

        public RdvConfirmeStatConsumer(SnapshotService snapshotService)
            => _snapshotService = snapshotService;

        public async Task Consume(ConsumeContext<RdvConfirmeStatEvent> context)
        {
            var evt = context.Message;
            var date = DateOnly.FromDateTime(evt.DateRdv.UtcDateTime);
            var snapshot = await _snapshotService
                .ObtenirOuCreerAsync(evt.CabinetId, date, context.CancellationToken);

            snapshot.AjouterRdv(confirme: true);
        }
    }
}
