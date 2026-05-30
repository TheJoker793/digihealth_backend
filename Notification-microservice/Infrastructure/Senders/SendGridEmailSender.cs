using Notification_microservice.Domain.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notification_microservice.Infrastructure.Senders
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ISendGridClient _client;
        private readonly string _expediteur;
        private readonly string _nomExpediteur;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(
        ISendGridClient client,
        IConfiguration config,
        ILogger<SendGridEmailSender> logger)
        {
            _client = client;
            _expediteur = config["SendGrid:From"] ?? "noreply@digihealth.tn";
            _nomExpediteur = config["SendGrid:FromName"] ?? "DigiHealth";
            _logger = logger;
        }
        public async Task<SendResult> EnvoyerAsync(
        string destinataire,
        string sujet,
        string corpsHtml,
        string? pieceJointeChemin = null,
        CancellationToken ct = default)
        {
            try
            {
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_expediteur, _nomExpediteur),
                    Subject = sujet,
                };

                msg.AddTo(new EmailAddress(destinataire));
                msg.AddContent(MimeType.Html, corpsHtml);

                // Pièce jointe (PDF ordonnance, compte-rendu…)
                if (!string.IsNullOrWhiteSpace(pieceJointeChemin)
                    && File.Exists(pieceJointeChemin))
                {
                    var bytes = await File.ReadAllBytesAsync(pieceJointeChemin, ct);
                    var base64 = Convert.ToBase64String(bytes);
                    var nomFichier = Path.GetFileName(pieceJointeChemin);

                    msg.AddAttachment(nomFichier, base64, "application/pdf");
                }

                var response = await _client.SendEmailAsync(msg, ct);

                if (response.IsSuccessStatusCode)
                {
                    var messageId = response.Headers
                        .GetValues("X-Message-Id").FirstOrDefault();

                    _logger.LogInformation(
                        "Email envoyé à {Dest} — MessageId: {Id}", destinataire, messageId);

                    return new SendResult(true, ProviderMessageId: messageId);
                }

                var body = await response.Body.ReadAsStringAsync(ct);
                _logger.LogWarning("SendGrid erreur {Status}: {Body}",
                    response.StatusCode, body);

                return new SendResult(false, Erreur: $"SendGrid {response.StatusCode}: {body}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur SendGrid pour {Dest}", destinataire);
                return new SendResult(false, Erreur: ex.Message);
            }
        }
    }
}
