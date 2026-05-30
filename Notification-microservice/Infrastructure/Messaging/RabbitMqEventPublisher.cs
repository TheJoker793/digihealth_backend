using MassTransit;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IBus _bus;
        private readonly ILogger<RabbitMqEventPublisher> _logger;

        public RabbitMqEventPublisher(IBus bus, ILogger<RabbitMqEventPublisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
            where TEvent : class
        {
            try
            {
                await _bus.Publish(@event, ct);
                _logger.LogDebug("Event publié : {Type}", typeof(TEvent).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur publication event {Type}", typeof(TEvent).Name);
                throw;
            }
        }
    }
}
