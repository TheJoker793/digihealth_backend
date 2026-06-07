using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Domain.Entities
{
    // ═══════════════════════════════════════════════════════════
    // AbonnementRapport — envoi automatique planifié
    // ═══════════════════════════════════════════════════════════
    public class AbonnementRapport : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public Guid CabinetId { get; private set; }
        public Guid CreePar { get; private set; }           // MedecinId

        // ═══════════════════════════════════════════
        // CONFIGURATION
        // ═══════════════════════════════════════════
        public TypeRapport TypeRapport { get; private set; }
        public FrequenceRapport Frequence { get; private set; }
        public string[] Destinataires { get; private set; } = [];  // emails
        public bool EstActif { get; private set; }

        // ═══════════════════════════════════════════
        // PLANIFICATION
        // ═══════════════════════════════════════════
        public DateTimeOffset? DernierEnvoi { get; private set; }
        public DateTimeOffset? ProchainEnvoi { get; private set; }

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static AbonnementRapport Create(
            Guid cabinetId,
            Guid creePar,
            TypeRapport typeRapport,
            FrequenceRapport frequence,
            string[] destinataires)
        {
            if (destinataires.Length == 0)
                throw new ArgumentException("Au moins un destinataire est obligatoire.");

            var abonnement = new AbonnementRapport
            {
                CabinetId = cabinetId,
                CreePar = creePar,
                TypeRapport = typeRapport,
                Frequence = frequence,
                Destinataires = destinataires,
                EstActif = true,
            };

            abonnement.ProchainEnvoi = abonnement.CalculerProchainEnvoi(DateTimeOffset.UtcNow);
            return abonnement;
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════

        /// <summary>
        /// Calcule la prochaine date d'envoi selon la fréquence.
        /// Quotidien → demain 07:00, Hebdo → lundi prochain 07:00,
        /// Mensuel → 1er du mois prochain 07:00.
        /// </summary>
        public DateTimeOffset CalculerProchainEnvoi(DateTimeOffset depuis)
        {
            var base_ = depuis.Date;

            return Frequence switch
            {
                FrequenceRapport.Quotidien =>
                    new DateTimeOffset(base_.AddDays(1).AddHours(7), depuis.Offset),

                FrequenceRapport.Hebdomadaire =>
                    new DateTimeOffset(
                        ProchainLundi(base_).AddHours(7), depuis.Offset),

                FrequenceRapport.Mensuel =>
                    new DateTimeOffset(
                        new DateTime(base_.Year, base_.Month, 1)
                            .AddMonths(1).AddHours(7),
                        depuis.Offset),

                _ => throw new NotSupportedException($"Fréquence {Frequence} non supportée.")
            };
        }

        /// <summary>Enregistre l'envoi et calcule le prochain.</summary>
        public void MarquerEnvoye()
        {
            DernierEnvoi = DateTimeOffset.UtcNow;
            ProchainEnvoi = CalculerProchainEnvoi(DernierEnvoi.Value);
            MarkUpdated();
        }

        public void Activer()
        {
            EstActif = true;
            if (ProchainEnvoi == null || ProchainEnvoi < DateTimeOffset.UtcNow)
                ProchainEnvoi = CalculerProchainEnvoi(DateTimeOffset.UtcNow);
            MarkUpdated();
        }

        public void Desactiver()
        {
            EstActif = false;
            MarkUpdated();
        }

        public void MettreAJourDestinataires(string[] destinataires)
        {
            if (destinataires.Length == 0)
                throw new ArgumentException("Au moins un destinataire est obligatoire.");

            Destinataires = destinataires;
            MarkUpdated();
        }

        public bool EstEchu()
            => EstActif
               && ProchainEnvoi.HasValue
               && ProchainEnvoi <= DateTimeOffset.UtcNow;

        // ── Helper ──────────────────────────────────────────────
        private static DateTime ProchainLundi(DateTime depuis)
        {
            var jours = ((int)DayOfWeek.Monday - (int)depuis.DayOfWeek + 7) % 7;
            return depuis.AddDays(jours == 0 ? 7 : jours);
        }
    }

}
