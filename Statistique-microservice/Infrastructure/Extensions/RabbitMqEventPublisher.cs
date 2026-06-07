using MassTransit;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Extensions
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
            try { await _bus.Publish(@event, ct); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur publication {Type}", typeof(TEvent).Name);
                throw;
            }
        }
    }
}
