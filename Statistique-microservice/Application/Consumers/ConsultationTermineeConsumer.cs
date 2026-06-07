using MassTransit;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Application.Consumers
{
    public class ConsultationTermineeConsumer : IConsumer<ConsultationTermineeEvent>
    {
        private readonly SnapshotService _snapshotService;

        public ConsultationTermineeConsumer(SnapshotService snapshotService)
            => _snapshotService = snapshotService;
        public async Task Consume(ConsumeContext<ConsultationTermineeEvent> context)
        {
            var evt = context.Message;
            var date = DateOnly.FromDateTime(evt.Date.UtcDateTime);
            var snapshot = await _snapshotService
                .ObtenirOuCreerAsync(evt.CabinetId, date, context.CancellationToken);

            snapshot.AjouterConsultation();

            if (evt.EstNouveauPatient)
                snapshot.AjouterNouveauPatient();
            else
                snapshot.AjouterPatientExistant();
        }
    }
}
