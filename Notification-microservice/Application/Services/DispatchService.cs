using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Interfaces;
using System.Diagnostics;

namespace Notification_microservice.Application.Services
{
    public class DispatchService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IPushSender _pushSender;
        private readonly IUnitOfWork _uow;

        public DispatchService(
            IEmailSender emailSender,
            ISmsSender smsSender,
            IPushSender pushSender,
            IUnitOfWork uow)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _pushSender = pushSender;
            _uow = uow;
        }

        public async Task DispatcherAsync(
        Notification notification,
        CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();
            SendResult result;

            try
            {
                result = notification.Canal switch
                {
                    CanalEnvoi.Email => await EnvoyerEmailAsync(notification, ct),
                    CanalEnvoi.SMS => await EnvoyerSmsAsync(notification, ct),
                    CanalEnvoi.Push => await EnvoyerPushAsync(notification, ct),
                    CanalEnvoi.InApp => SimulerInApp(notification),
                    _ => throw new NotSupportedException($"Canal {notification.Canal} non supporté.")
                };
            }
            catch (Exception ex)
            {
                result = new SendResult(false, Erreur: ex.Message);
            }

            sw.Stop();

            // Mettre à jour l'agrégat selon le résultat
            if (result.Succes)
            {
                notification.MarquerEnvoyee(result.ProviderMessageId);
            }
            else
            {
                notification.MarquerEchouee(result.Erreur ?? "Erreur inconnue");
            }

            _uow.Notifications.Update(notification);
        }

        // ── Envois par canal ─────────────────────────────────────

        private async Task<SendResult> EnvoyerEmailAsync(
            Notification n, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(n.ContactEmail))
                return new SendResult(false, Erreur: "Email du destinataire manquant.");

            return await _emailSender.EnvoyerAsync(
                n.ContactEmail,
                n.Sujet,
                n.CorpsRendu,
                n.PieceJointeChemin,
                ct);
        }

        private async Task<SendResult> EnvoyerSmsAsync(
            Notification n, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(n.ContactTelephone))
                return new SendResult(false, Erreur: "Téléphone du destinataire manquant.");

            // Pour SMS : utiliser le corps texte brut (sans HTML)
            var corpsTexte = StripperHtml(n.CorpsRendu);

            return await _smsSender.EnvoyerAsync(n.ContactTelephone, corpsTexte, ct);
        }

        private async Task<SendResult> EnvoyerPushAsync(
            Notification n, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(n.TokenFcm))
                return new SendResult(false, Erreur: "Token FCM du destinataire manquant.");

            return await _pushSender.EnvoyerAsync(
                n.TokenFcm,
                n.Sujet,
                StripperHtml(n.CorpsRendu),
                data: new Dictionary<string, string>
                {
                    ["notificationId"] = n.Id.ToString(),
                    ["typeEvenement"] = n.TypeEvenement.ToString(),
                    ["sourceId"] = n.SourceId?.ToString() ?? string.Empty,
                },
                ct);
        }

        private static SendResult SimulerInApp(Notification n)
        {
            // InApp : la notification est créée en base, le front la lit via SignalR.
            // L'envoi est considéré réussi dès la persistance.
            return new SendResult(true, ProviderMessageId: $"inapp:{n.Id}");
        }

        // ── Helpers ──────────────────────────────────────────────

        private static string StripperHtml(string html)
        {
            // Suppression basique des balises HTML pour SMS/Push
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", " ")
                .Replace("  ", " ").Trim();
        }
    }
}
