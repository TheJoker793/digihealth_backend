namespace Notification_microservice.Domain.Interfaces
{
    public interface INumeroNotificationGenerator
    {
        Task<string> GenererAsync(CancellationToken ct = default);

    }
}
