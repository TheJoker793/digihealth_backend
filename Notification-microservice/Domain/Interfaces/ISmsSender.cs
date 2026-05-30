using System.IO.Pipelines;

namespace Notification_microservice.Domain.Interfaces
{
    public interface ISmsSender
    {
        Task<SendResult> EnvoyerAsync(
            string numeroDest,        // format international : +21622334455
            string corps,
            CancellationToken ct = default);
    }
}
