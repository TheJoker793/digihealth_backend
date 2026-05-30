using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Entities
{
    public class TemplateNotification:BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════

        /// <summary>Code lisible unique — ex: "rdv_confirme_email_fr"</summary>
        public string Code { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // PORTÉE
        // ═══════════════════════════════════════════
        public TypeEvenement TypeEvenement { get; private set; }
        public CanalEnvoi Canal { get; private set; }
        public string Langue { get; private set; } = default!;   // "fr" | "ar" | "en"
        public bool EstActif { get; private set; }

        // ═══════════════════════════════════════════
        // CONTENU (syntaxe Scriban : {{variable}})
        // ═══════════════════════════════════════════
        public string SujetTemplate { get; private set; } = default!;
        public string CorpsTemplate { get; private set; } = default!;
        public string[] Variables { get; private set; } = [];    // liste des variables attendues

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static TemplateNotification Create(
            string code,
            TypeEvenement typeEvenement,
            CanalEnvoi canal,
            string langue,
            string sujetTemplate,
            string corpsTemplate,
            string[] variables)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Le code est obligatoire.");

            if (string.IsNullOrWhiteSpace(sujetTemplate))
                throw new ArgumentException("Le sujet template est obligatoire.");

            if (string.IsNullOrWhiteSpace(corpsTemplate))
                throw new ArgumentException("Le corps template est obligatoire.");

            return new TemplateNotification
            {
                Code = code.ToLowerInvariant().Trim(),
                TypeEvenement = typeEvenement,
                Canal = canal,
                Langue = langue.ToLowerInvariant(),
                SujetTemplate = sujetTemplate,
                CorpsTemplate = corpsTemplate,
                Variables = variables,
                EstActif = true,
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════

        /// <summary>
        /// Remplace les {{variable}} dans le sujet et le corps.
        /// Les variables non fournies restent vides (pas d'exception).
        /// </summary>
        public (string sujet, string corps) Rendre(Dictionary<string, string> valeurs)
        {
            if (!EstActif)
                throw new TemplateInactifException(Code);

            var sujet = SujetTemplate;
            var corps = CorpsTemplate;

            foreach (var (cle, valeur) in valeurs)
            {
                sujet = sujet.Replace($"{{{{{cle}}}}}", valeur);
                corps = corps.Replace($"{{{{{cle}}}}}", valeur);
            }

            return (sujet, corps);
        }

        public void MettreAJour(
            string sujetTemplate,
            string corpsTemplate,
            string[] variables)
        {
            SujetTemplate = sujetTemplate;
            CorpsTemplate = corpsTemplate;
            Variables = variables;
            MarkUpdated();
        }

        public void Activer()
        {
            EstActif = true;
            MarkUpdated();
        }

        public void Desactiver()
        {
            EstActif = false;
            MarkUpdated();
        }
    }
}
