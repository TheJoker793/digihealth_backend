namespace Facturation_microservice.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T domainEvent);

    }
}
