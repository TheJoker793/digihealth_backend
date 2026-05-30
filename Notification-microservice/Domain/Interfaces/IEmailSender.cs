using System.IO.Pipelines;

namespace Notification_microservice.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task<SendResult> EnvoyerAsync(
            string destinataire,
            string sujet,
            string corpsHtml,
            string? pieceJointeChemin = null,
            CancellationToken ct = default);
    }
}
