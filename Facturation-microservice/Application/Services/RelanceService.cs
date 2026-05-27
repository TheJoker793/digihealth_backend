using Facturation_microservice.Domain.Events;
using Facturation_microservice.Domain.Interfaces;

namespace Facturation_microservice.Application.Services
{
    public class RelanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _publisher;

        public async Task VerifierRetardsAsync()
        {
            var factures = await _unitOfWork.Factures.GetEnRetardAsync();

            foreach (var f in factures)
            {
                await _publisher.PublishAsync(
                    new FactureEnRetardEvent(f.Id, f.PatientId)
                );
            }
        }
    }
}
