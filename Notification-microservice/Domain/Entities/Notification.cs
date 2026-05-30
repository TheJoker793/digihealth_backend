using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Events;
using static Notification_microservice.Domain.Exceptions.NotificationExceptions;

namespace Notification_microservice.Domain.Entities
{
    public class Notification:BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public string Numero { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // ÉVÉNEMENT SOURCE
        // ═══════════════════════════════════════════
        public TypeEvenement TypeEvenement { get; private set; }
        public Guid? SourceId { get; private set; }       // ex: RdvId, DocumentId, PrescriptionId

        // ═══════════════════════════════════════════
        // DESTINATAIRE
        // ═══════════════════════════════════════════
        public Guid DestinataireId { get; private set; }
        public string TypeDestinataire { get; private set; } = default!;  // "Patient" | "Medecin"
        public string? ContactEmail { get; private set; }
        public string? ContactTelephone { get; private set; }
        public string? TokenFcm { get; private set; }     // token Firebase pour push

        // ═══════════════════════════════════════════
        // CONTENU RENDU
        // ═══════════════════════════════════════════
        public CanalEnvoi Canal { get; private set; }
        public string Sujet { get; private set; } = default!;
        public string CorpsRendu { get; private set; } = default!;  // HTML ou texte après rendu du template
        public string? PieceJointeChemin { get; private set; }      // chemin MinIO (ex: ordonnance PDF)

        // ═══════════════════════════════════════════
        // STATUT ET TENTATIVES
        // ═══════════════════════════════════════════
        public StatutNotification Statut { get; private set; }
        public int NbTentatives { get; private set; }
        public int MaxTentatives { get; private set; }
        public DateTimeOffset? DateEnvoi { get; private set; }
        public DateTimeOffset? DateProgrammee { get; private set; }  // envoi différé (plage horaire)
        public string? DerniereErreur { get; private set; }

        // ═══════════════════════════════════════════
        // HISTORIQUE (composition)
        // ═══════════════════════════════════════════
        private readonly List<HistoriqueEnvoi> _historiques = new();
        public IReadOnlyCollection<HistoriqueEnvoi> Historiques => _historiques.AsReadOnly();

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static Notification Create(
            string numero,
            TypeEvenement typeEvenement,
            Guid destinataireId,
            string typeDestinataire,
            CanalEnvoi canal,
            string sujet,
            string corpsRendu,
            Guid? sourceId = null,
            string? contactEmail = null,
            string? contactTelephone = null,
            string? tokenFcm = null,
            string? pieceJointeChemin = null,
            DateTimeOffset? dateProgrammee = null,
            int maxTentatives = 3)
        {
            if (string.IsNullOrWhiteSpace(sujet))
                throw new ArgumentException("Le sujet est obligatoire.");

            if (string.IsNullOrWhiteSpace(corpsRendu))
                throw new ArgumentException("Le corps est obligatoire.");

            var notif = new Notification
            {
                Numero = numero,
                TypeEvenement = typeEvenement,
                SourceId = sourceId,
                DestinataireId = destinataireId,
                TypeDestinataire = typeDestinataire,
                Canal = canal,
                Sujet = sujet,
                CorpsRendu = corpsRendu,
                ContactEmail = contactEmail,
                ContactTelephone = contactTelephone,
                TokenFcm = tokenFcm,
                PieceJointeChemin = pieceJointeChemin,
                Statut = StatutNotification.EnAttente,
                NbTentatives = 0,
                MaxTentatives = maxTentatives,
                DateProgrammee = dateProgrammee,
            };

            return notif;
        }

        // ═══════════════════════════════════════════
        // CYCLE DE VIE
        // ═══════════════════════════════════════════

        /// <summary>Marque la notification comme envoyée avec succès.</summary>
        public void MarquerEnvoyee(string? providerMessageId = null)
        {
            if (Statut == StatutNotification.Annule)
                throw new NotificationAnnuleeException(Id);

            Statut = StatutNotification.Envoye;
            DateEnvoi = DateTimeOffset.UtcNow;
            NbTentatives++;
            DerniereErreur = null;
            MarkUpdated();

            var historique = HistoriqueEnvoi.Create(
                Id, Canal, ResultatEnvoi.Succes,
                providerResponse: providerMessageId);

            _historiques.Add(historique);

            AddDomainEvent(new NotificationEnvoyeeEvent(
                Id, DestinataireId, TypeEvenement, Canal));
        }

        /// <summary>Enregistre une tentative échouée. Passe en Echoue si max atteint.</summary>
        public void MarquerEchouee(string messageErreur)
        {
            if (Statut == StatutNotification.Annule)
                throw new NotificationAnnuleeException(Id);

            NbTentatives++;
            DerniereErreur = messageErreur;

            var historique = HistoriqueEnvoi.Create(
                Id, Canal, ResultatEnvoi.Echec,
                messageErreur: messageErreur);

            _historiques.Add(historique);

            if (NbTentatives >= MaxTentatives)
            {
                Statut = StatutNotification.Echoue;
                AddDomainEvent(new NotificationEchoueeEvent(
                    Id, DestinataireId, TypeEvenement, Canal, messageErreur));
            }
            else
            {
                // Reste en EnAttente pour le prochain retry
                Statut = StatutNotification.EnAttente;
            }

            MarkUpdated();
        }

        /// <summary>Programme l'envoi à une heure future (respect des plages horaires).</summary>
        public void Programmer(DateTimeOffset dateHeure)
        {
            if (Statut != StatutNotification.EnAttente)
                throw new InvalidOperationException("Seule une notification en attente peut être programmée.");

            DateProgrammee = dateHeure;
            MarkUpdated();
        }

        /// <summary>Annule définitivement la notification (opt-out, RDV annulé…).</summary>
        public void Annuler(string motif)
        {
            if (Statut == StatutNotification.Envoye)
                throw new InvalidOperationException("Impossible d'annuler une notification déjà envoyée.");

            Statut = StatutNotification.Annule;
            DerniereErreur = $"Annulée : {motif}";
            MarkUpdated();
        }

        /// <summary>Remet en attente pour un nouveau retry (appelé par le job Hangfire).</summary>
        public void Reessayer()
        {
            if (Statut != StatutNotification.Echoue)
                throw new InvalidOperationException("Seule une notification échouée peut être réessayée.");

            if (NbTentatives >= MaxTentatives)
                throw new MaxTentativesAtteinteException(Id, MaxTentatives);

            Statut = StatutNotification.EnAttente;
            MarkUpdated();
        }

        // ═══════════════════════════════════════════
        // REQUÊTES MÉTIER
        // ═══════════════════════════════════════════
        public bool PeutEtreEnvoyee()
            => Statut == StatutNotification.EnAttente
               && (DateProgrammee == null || DateProgrammee <= DateTimeOffset.UtcNow);

        public bool EstEpuisee()
            => NbTentatives >= MaxTentatives;

    }
}
