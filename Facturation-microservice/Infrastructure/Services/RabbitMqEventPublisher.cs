using Facturation_microservice.Domain.Interfaces;
using MassTransit;

namespace Facturation_microservice.Infrastructure.Services
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T domainEvent)
        {
            await _publishEndpoint.Publish(domainEvent!);
        }
    }
}
