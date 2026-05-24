using MassTransit;
using Prescription_microservice.Domain.Interfaces;

namespace Prescription_microservice.Infrastructure.Services
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishAsync<T>(T domainEvent, CancellationToken ct = default) where T : class
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            await _publishEndpoint.Publish(domainEvent, ct);
        }
    }
}
