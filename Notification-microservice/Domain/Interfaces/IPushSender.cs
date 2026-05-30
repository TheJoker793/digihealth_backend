namespace Notification_microservice.Domain.Interfaces
{
    public interface IPushSender
    {
        Task<SendResult> EnvoyerAsync(
            string tokenFcm,
            string titre,
            string corps,
            Dictionary<string, string>? data = null,
            CancellationToken ct = default);
    }
}
