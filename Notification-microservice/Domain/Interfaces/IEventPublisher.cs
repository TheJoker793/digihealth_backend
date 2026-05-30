namespace Notification_microservice.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
            where TEvent : class;
    }
}
