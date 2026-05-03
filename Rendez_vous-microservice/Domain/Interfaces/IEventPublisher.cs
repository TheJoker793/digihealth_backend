namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(object domainEvent);
    }
}
