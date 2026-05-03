using MassTransit;
using Patient_microservice.Domain.Interfaces;
namespace Patient_microservice.Services
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T domainEvent) where T : class
        {
            await _publishEndpoint.Publish(domainEvent);
        }
    }
}
