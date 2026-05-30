using Notification_microservice.Domain.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notification_microservice.Infrastructure.Senders
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly string _from;
        private readonly ILogger<TwilioSmsSender> _logger;

        public TwilioSmsSender(IConfiguration config, ILogger<TwilioSmsSender> logger)
        {
            _logger = logger;

            var accountSid = config["Twilio:AccountSid"]
                ?? throw new InvalidOperationException("Twilio:AccountSid manquant.");
            var authToken = config["Twilio:AuthToken"]
                ?? throw new InvalidOperationException("Twilio:AuthToken manquant.");

            _from = config["Twilio:From"]
                ?? throw new InvalidOperationException("Twilio:From manquant.");

            TwilioClient.Init(accountSid, authToken);
        }
        public async Task<SendResult> EnvoyerAsync(
        string numeroDest,
        string corps,
        CancellationToken ct = default)
        {
            try
            {
                // SMS : 160 caractères max — tronquer si nécessaire
                var corpsLimite = corps.Length > 160
                    ? corps[..157] + "..."
                    : corps;

                var message = await MessageResource.CreateAsync(
                    to: new PhoneNumber(numeroDest),
                    from: new PhoneNumber(_from),
                    body: corpsLimite);

                _logger.LogInformation(
                    "SMS envoyé à {Dest} — SID: {Sid}", numeroDest, message.Sid);

                return new SendResult(true, ProviderMessageId: message.Sid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Twilio pour {Dest}", numeroDest);
                return new SendResult(false, Erreur: ex.Message);
            }
        }
    }
}
