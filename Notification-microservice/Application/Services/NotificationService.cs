using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.DTOs.Responses;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Interfaces;
using static Notification_microservice.Domain.Exceptions.NotificationExceptions;

namespace Notification_microservice.Application.Services
{
    public class NotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly DispatchService _dispatch;
        private readonly INumeroNotificationGenerator _numeroGen;
        private readonly IEventPublisher _publisher;

        public NotificationService(
        IUnitOfWork uow,
        DispatchService dispatch,
        INumeroNotificationGenerator numeroGen,
        IEventPublisher publisher)
        {
            _uow = uow;
            _dispatch = dispatch;
            _numeroGen = numeroGen;
            _publisher = publisher;
        }

        // ═══════════════════════════════════════════
        // CRÉER ET ENVOYER une notification
        // ═══════════════════════════════════════════
        public async Task<NotificationResponse> EnvoyerAsync(
            EnvoyerNotificationRequest request,
            CancellationToken ct = default)
        {
            // 1. Vérifier les préférences du destinataire
            var preference = await _uow.Preferences
                .GetByDestinataireAsync(request.DestinataireId, request.TypeDestinataire, ct);

            if (preference is not null)
            {
                if (preference.EstOptOut)
                    throw new DestinataireOptOutException(request.DestinataireId);

                if (!preference.AccepteCanal(request.Canal))
                    throw new CanalNonAutoriseException(
                        request.DestinataireId, request.Canal.ToString());
            }

            // 2. Résoudre le template
            var langue = preference?.Langue ?? "fr";
            var template = await _uow.Templates
                .GetByEvenementEtCanalAsync(request.TypeEvenement, request.Canal, langue, ct)
                ?? throw new TemplateIntrouvableException(
                    $"{request.TypeEvenement}_{request.Canal}_{langue}");

            // 3. Rendre le template avec les variables
            var (sujetRendu, corpsRendu) = template.Rendre(request.Variables);

            // 4. Générer un numéro unique
            var numero = await _numeroGen.GenererAsync(ct);

            // 5. Vérifier la plage horaire → éventuellement programmer
            DateTimeOffset? dateProgrammee = request.DateProgrammee;
            if (preference is not null && dateProgrammee == null)
            {
                var heureLocale = TimeOnly.FromDateTime(DateTimeOffset.UtcNow.LocalDateTime);
                if (!preference.EstDansPlageHoraire(heureLocale))
                    dateProgrammee = preference.ProchainEnvoiAutorise();
            }

            // 6. Créer l'agrégat
            var notification = Notification.Create(
                numero,
                request.TypeEvenement,
                request.DestinataireId,
                request.TypeDestinataire,
                request.Canal,
                sujetRendu,
                corpsRendu,
                sourceId: request.SourceId,
                contactEmail: request.ContactEmail,
                contactTelephone: request.ContactTelephone,
                tokenFcm: request.TokenFcm,
                pieceJointeChemin: request.PieceJointeChemin,
                dateProgrammee: dateProgrammee);

            await _uow.Notifications.AddAsync(notification, ct);
            await _uow.SaveChangesAsync(ct);

            // 7. Envoyer immédiatement si pas programmée
            if (notification.PeutEtreEnvoyee())
                await _dispatch.DispatcherAsync(notification, ct);

            // 8. Publier les domain events
            foreach (var evt in notification.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            notification.ClearDomainEvents();

            return ToResponse(notification);
        }

        // ═══════════════════════════════════════════
        // RÉESSAYER les notifications échouées (appelé par Hangfire)
        // ═══════════════════════════════════════════
        public async Task ReessayerEchoueesAsync(CancellationToken ct = default)
        {
            var echouees = await _uow.Notifications.GetEchoueesAsync(ct: ct);

            foreach (var notification in echouees)
            {
                try
                {
                    notification.Reessayer();
                    await _dispatch.DispatcherAsync(notification, ct);

                    foreach (var evt in notification.DomainEvents)
                        await _publisher.PublishAsync(evt, ct);
                    notification.ClearDomainEvents();
                }
                catch (MaxTentativesAtteinteException)
                {
                    // Déjà marquée Echoue dans l'entité — on laisse passer
                }
            }

            await _uow.SaveChangesAsync(ct);
        }

        // ═══════════════════════════════════════════
        // ENVOYER LES PROGRAMMÉES (appelé par Hangfire toutes les minutes)
        // ═══════════════════════════════════════════
        public async Task EnvoyerProgrammeesAsync(CancellationToken ct = default)
        {
            var enAttente = await _uow.Notifications.GetEnAttenteAsync(ct: ct);
            var aEnvoyer = enAttente.Where(n => n.PeutEtreEnvoyee());

            foreach (var notification in aEnvoyer)
            {
                await _dispatch.DispatcherAsync(notification, ct);

                foreach (var evt in notification.DomainEvents)
                    await _publisher.PublishAsync(evt, ct);
                notification.ClearDomainEvents();
            }

            await _uow.SaveChangesAsync(ct);
        }

        // ═══════════════════════════════════════════
        // ANNULER une notification non encore envoyée
        // ═══════════════════════════════════════════
        public async Task AnnulerAsync(
            Guid notificationId,
            string motif,
            CancellationToken ct = default)
        {
            var notification = await _uow.Notifications.GetByIdAsync(notificationId, ct)
                ?? throw new NotificationIntrouvableException(notificationId);

            notification.Annuler(motif);
            _uow.Notifications.Update(notification);
            await _uow.SaveChangesAsync(ct);
        }

        // ═══════════════════════════════════════════
        // LECTURE
        // ═══════════════════════════════════════════
        public async Task<NotificationResponse> GetByIdAsync(
            Guid id, CancellationToken ct = default)
        {
            var n = await _uow.Notifications.GetAvecHistoriquesAsync(id, ct)
                ?? throw new NotificationIntrouvableException(id);

            return ToResponse(n, avecHistoriques: true);
        }

        public async Task<IEnumerable<NotificationResponse>> GetByDestinataireAsync(
            Guid destinataireId, CancellationToken ct = default)
        {
            var notifs = await _uow.Notifications.GetByDestinataireAsync(destinataireId, ct);
            return notifs.Select(n => ToResponse(n));
        }

        // ═══════════════════════════════════════════
        // MAPPING
        // ═══════════════════════════════════════════
        private static NotificationResponse ToResponse(
            Notification n, bool avecHistoriques = false)
        {
            IEnumerable<HistoriqueEnvoiResponse>? historiques = null;

            if (avecHistoriques)
            {
                historiques = n.Historiques.Select(h => new HistoriqueEnvoiResponse(
                    h.Id, h.Canal, h.Resultat,
                    h.MessageErreur, h.ProviderResponse,
                    h.DureeMs, h.DateTentative));
            }

            return new NotificationResponse(
                n.Id, n.Numero, n.TypeEvenement,
                n.DestinataireId, n.TypeDestinataire,
                n.Canal, n.Sujet, n.Statut,
                n.NbTentatives, n.MaxTentatives,
                n.DateEnvoi, n.DateProgrammee,
                n.DerniereErreur, n.CreatedAt,
                historiques);
        }
    }
}
