using Facturation_microservice.Domain.Enums;
using Facturation_microservice.Domain.Events;
using Facturation_microservice.Domain.Interfaces;

namespace Facturation_microservice.Infrastructure.Services
{
    public class HangfireRelanceJob
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventPublisher _publisher;

        public HangfireRelanceJob(
            IUnitOfWork uow,
            IEventPublisher publisher)
        {
            _uow = uow;
            _publisher = publisher;
        }

        public async Task ExecuteAsync()
        {
            var factures = await _uow.Factures.GetEnRetardAsync();

            foreach (var facture in factures)
            {
                facture.Statut = StatutFacture.EnRetard;

                await _publisher.PublishAsync(
                    new FactureEnRetardEvent(
                        facture.Id,
                        facture.PatientId
                    )
                );
            }

            await _uow.SaveChangesAsync();
        }
    }
}
