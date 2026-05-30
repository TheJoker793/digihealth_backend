namespace Notification_microservice.Domain.Exceptions
{
    public class NotificationExceptions
    {
        public class NotificationIntrouvableException(Guid id)
    : Exception($"Notification {id} introuvable.");

        public class NotificationAnnuleeException(Guid id)
            : Exception($"La notification {id} est annulée — opération interdite.");

        public class MaxTentativesAtteinteException(Guid id, int max)
            : Exception($"Notification {id} : maximum de {max} tentatives atteint.");

        public class TemplateIntrouvableException(string code)
            : Exception($"Template de notification '{code}' introuvable ou inactif.");

        public class TemplateInactifException(string code)
            : Exception($"Le template '{code}' est désactivé.");

        public class DestinataireOptOutException(Guid destinataireId)
            : Exception($"Le destinataire {destinataireId} a refusé toutes les notifications (opt-out).");

        public class CanalNonAutoriseException(Guid destinataireId, string canal)
            : Exception($"Le destinataire {destinataireId} a désactivé le canal {canal}.");

        public class HorsPlageHoraireException(Guid notificationId, DateTimeOffset prochainEnvoi)
            : Exception($"Notification {notificationId} hors plage horaire. Prochain envoi : {prochainEnvoi:f}.");

    }
}
