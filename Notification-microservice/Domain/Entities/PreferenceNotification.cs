using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Entities
{
    public class PreferenceNotification:BaseEntity
    {
        // ═══════════════════════════════════════════
        // DESTINATAIRE
        // ═══════════════════════════════════════════
        public Guid DestinataireId { get; private set; }
        public string TypeDestinataire { get; private set; } = default!;  // "Patient" | "Medecin"

        // ═══════════════════════════════════════════
        // CANAUX ACTIFS
        // ═══════════════════════════════════════════
        public CanalEnvoi[] CanauxActifs { get; private set; } = [];

        // ═══════════════════════════════════════════
        // PLAGE HORAIRE AUTORISÉE
        // ═══════════════════════════════════════════
        public TimeOnly? HeureDebut { get; private set; }  // ex: 08:00
        public TimeOnly? HeureFin { get; private set; }    // ex: 20:00

        // ═══════════════════════════════════════════
        // OPT-OUT
        // ═══════════════════════════════════════════
        public bool EstOptOut { get; private set; }       // opt-out global — aucune notification
        public CanalEnvoi[] CanauxOptOut { get; private set; } = [];  // opt-out par canal

        // ═══════════════════════════════════════════
        // LANGUE PRÉFÉRÉE
        // ═══════════════════════════════════════════
        public string Langue { get; private set; } = "fr";

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static PreferenceNotification Create(
            Guid destinataireId,
            string typeDestinataire,
            CanalEnvoi[]? canauxActifs = null,
            string langue = "fr")
        {
            return new PreferenceNotification
            {
                DestinataireId = destinataireId,
                TypeDestinataire = typeDestinataire,
                CanauxActifs = canauxActifs ?? [CanalEnvoi.Email, CanalEnvoi.SMS],
                Langue = langue,
                EstOptOut = false,
                CanauxOptOut = [],
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════

        /// <summary>Vérifie si le canal est autorisé pour ce destinataire.</summary>
        public bool AccepteCanal(CanalEnvoi canal)
        {
            if (EstOptOut) return false;
            if (CanauxOptOut.Contains(canal)) return false;
            return CanauxActifs.Contains(canal);
        }

        /// <summary>
        /// Vérifie si l'envoi est autorisé à l'heure actuelle (plage horaire).
        /// Si aucune plage définie, toujours autorisé.
        /// </summary>
        public bool EstDansPlageHoraire(TimeOnly heure)
        {
            if (HeureDebut == null || HeureFin == null) return true;
            return heure >= HeureDebut && heure <= HeureFin;
        }

        /// <summary>
        /// Retourne la prochaine heure d'envoi autorisée.
        /// Si dans la plage → maintenant. Sinon → demain à HeureDebut.
        /// </summary>
        public DateTimeOffset? ProchainEnvoiAutorise()
        {
            if (HeureDebut == null || HeureFin == null) return DateTimeOffset.UtcNow;

            var maintenant = DateTimeOffset.UtcNow;
            var heureMaintenant = TimeOnly.FromDateTime(maintenant.LocalDateTime);

            if (EstDansPlageHoraire(heureMaintenant))
                return maintenant;

            // Programmer au prochain début de plage (demain si dépassé)
            var dateBase = maintenant.Date;
            if (heureMaintenant > HeureFin)
                dateBase = dateBase.AddDays(1);

            return new DateTimeOffset(
                dateBase.Year, dateBase.Month, dateBase.Day,
                HeureDebut.Value.Hour, HeureDebut.Value.Minute, 0,
                maintenant.Offset);
        }

        public void DefinirPlageHoraire(TimeOnly debut, TimeOnly fin)
        {
            if (fin <= debut)
                throw new ArgumentException("L'heure de fin doit être après l'heure de début.");

            HeureDebut = debut;
            HeureFin = fin;
            MarkUpdated();
        }

        public void ActiverCanal(CanalEnvoi canal)
        {
            if (!CanauxActifs.Contains(canal))
                CanauxActifs = [.. CanauxActifs, canal];

            // Retirer de l'opt-out si présent
            CanauxOptOut = CanauxOptOut.Where(c => c != canal).ToArray();
            MarkUpdated();
        }

        public void DesactiverCanal(CanalEnvoi canal)
        {
            CanauxActifs = CanauxActifs.Where(c => c != canal).ToArray();

            if (!CanauxOptOut.Contains(canal))
                CanauxOptOut = [.. CanauxOptOut, canal];

            MarkUpdated();
        }

        public void OptOutTotal()
        {
            EstOptOut = true;
            MarkUpdated();
        }

        public void AnnulerOptOut()
        {
            EstOptOut = false;
            CanauxOptOut = [];
            MarkUpdated();
        }

        public void ChangerLangue(string langue)
        {
            Langue = langue.ToLowerInvariant();
            MarkUpdated();
        }

    }
}
