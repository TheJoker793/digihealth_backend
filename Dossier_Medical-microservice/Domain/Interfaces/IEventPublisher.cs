namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T domainEvent, CancellationToken ct = default) where T : class;
    }
}
