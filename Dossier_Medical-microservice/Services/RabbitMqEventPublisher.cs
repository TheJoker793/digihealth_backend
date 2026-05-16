using Dossier_Medical_microservice.Domain.Interfaces;
using MassTransit;

namespace Dossier_Medical_microservice.Services
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : class
        {
            await _publishEndpoint.Publish(@event, ct);
        }
    }
}
