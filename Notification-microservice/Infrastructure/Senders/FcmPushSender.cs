using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Senders
{
    public class FcmPushSender : IPushSender
    {
        private readonly FirebaseMessaging _messaging;
        private readonly ILogger<FcmPushSender> _logger;

        public FcmPushSender(FirebaseApp firebaseApp, ILogger<FcmPushSender> logger)
        {
            _messaging = FirebaseMessaging.GetMessaging(firebaseApp);
            _logger = logger;
        }
        public async Task<SendResult> EnvoyerAsync(
        string tokenFcm,
        string titre,
        string corps,
        Dictionary<string, string>? data = null,
        CancellationToken ct = default)
        {
            try
            {
                var message = new Message
                {
                    Token = tokenFcm,
                    Notification = new Notification
                    {
                        Title = titre,
                        Body = corps,
                    },
                    Data = data ?? new Dictionary<string, string>(),
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification
                        {
                            ChannelId = "digihealth_medical",
                            Sound = "default",
                        }
                    },
                    Apns = new ApnsConfig
                    {
                        Aps = new Aps { Sound = "default", Badge = 1 }
                    }
                };

                var messageId = await _messaging.SendAsync(message, ct);

                _logger.LogInformation(
                    "Push envoyé — FCM MessageId: {Id}", messageId);

                return new SendResult(true, ProviderMessageId: messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur FCM pour token {Token}", tokenFcm[..10] + "…");
                return new SendResult(false, Erreur: ex.Message);
            }
        }
    }
}
