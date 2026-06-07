using MassTransit;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Application.Consumers
{
    public class FactureGenereeConsumer : IConsumer<FactureGenereeStatEvent>
    {
        private readonly SnapshotService _snapshotService;

        public FactureGenereeConsumer(SnapshotService snapshotService)
            => _snapshotService = snapshotService;

        public async Task Consume(ConsumeContext<FactureGenereeStatEvent> context)
        {
            var evt = context.Message;
            var date = DateOnly.FromDateTime(evt.Date.UtcDateTime);
            var snapshot = await _snapshotService
                .ObtenirOuCreerAsync(evt.CabinetId, date, context.CancellationToken);

            snapshot.AjouterFacture(evt.Montant, evt.EstImpayee);
        }
    }
}
