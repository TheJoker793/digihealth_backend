using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Entities
{
    public class HistoriqueEnvoi:BaseEntity
    {
        // ═══════════════════════════════════════════
        // RELATION
        // ═══════════════════════════════════════════
        public Guid NotificationId { get; private set; }
        public Notification Notification { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // DÉTAIL DE LA TENTATIVE
        // ═══════════════════════════════════════════
        public CanalEnvoi Canal { get; private set; }
        public ResultatEnvoi Resultat { get; private set; }
        public string? MessageErreur { get; private set; }
        public string? ProviderResponse { get; private set; }  // ID message SendGrid / Twilio
        public DateTimeOffset DateTentative { get; private set; }
        public long DureeMs { get; private set; }              // durée de l'appel au provider

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static HistoriqueEnvoi Create(
            Guid notificationId,
            CanalEnvoi canal,
            ResultatEnvoi resultat,
            string? messageErreur = null,
            string? providerResponse = null,
            long dureeMs = 0)
        {
            return new HistoriqueEnvoi
            {
                NotificationId = notificationId,
                Canal = canal,
                Resultat = resultat,
                MessageErreur = messageErreur,
                ProviderResponse = providerResponse,
                DateTentative = DateTimeOffset.UtcNow,
                DureeMs = dureeMs,
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════
        public bool EstSucces() => Resultat == ResultatEnvoi.Succes;
    }
}
