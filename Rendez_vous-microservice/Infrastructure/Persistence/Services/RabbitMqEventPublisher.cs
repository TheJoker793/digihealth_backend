using MassTransit;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Services
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T @event) where T : class
        {
            await _publishEndpoint.Publish(@event);
        }

        public async Task PublishAsync(object domainEvent)
        {
            // MassTransit accepte object, mais il est préférable de caster en dynamic
            await _publishEndpoint.Publish((dynamic)domainEvent);
        }
    }
}
