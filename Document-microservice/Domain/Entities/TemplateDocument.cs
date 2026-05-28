using Document_microservice.Domain.Enums;

namespace Document_microservice.Domain.Entities
{
    public class TemplateDocument:BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public string Nom { get; private set; } = default!;
        public string? Description { get; private set; }

        // ═══════════════════════════════════════════
        // PORTÉE
        // ═══════════════════════════════════════════
        public TypeDocument TypeDocument { get; private set; }
        public string? Specialite { get; private set; }     // null = tous les médecins
        public Guid? CabinetId { get; private set; }        // null = template global plateforme
        public bool EstActif { get; private set; }

        // ═══════════════════════════════════════════
        // CONTENU
        // ═══════════════════════════════════════════
        public string ContenuHtml { get; private set; } = default!;  // Liquid / Handlebars
        public string[] Variables { get; private set; } = [];        // ex: ["patient.nom", "date", "diagnostic"]
        public string Version { get; private set; } = default!;      // "1.0", "1.1"…

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static TemplateDocument Create(
            string nom,
            TypeDocument typeDocument,
            string contenuHtml,
            string[] variables,
            string version = "1.0",
            string? specialite = null,
            Guid? cabinetId = null,
            string? description = null)
        {
            return new TemplateDocument
            {
                Nom = nom.Trim(),
                Description = description,
                TypeDocument = typeDocument,
                ContenuHtml = contenuHtml,
                Variables = variables,
                Version = version,
                Specialite = specialite,
                CabinetId = cabinetId,
                EstActif = true,
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════

        /// <summary>
        /// Remplace les variables {{nom}} dans le contenu HTML.
        /// Les valeurs non fournies restent vides.
        /// </summary>
        public string Rendre(Dictionary<string, string> valeurs)
        {
            var rendu = ContenuHtml;

            foreach (var (cle, valeur) in valeurs)
                rendu = rendu.Replace($"{{{{{cle}}}}}", valeur);

            return rendu;
        }

        public void MettreAJour(string nouveauContenu, string[] nouvellesVariables, string nouvelleVersion)
        {
            ContenuHtml = nouveauContenu;
            Variables = nouvellesVariables;
            Version = nouvelleVersion;
            MarkUpdated();
        }

        public void Desactiver()
        {
            EstActif = false;
            MarkUpdated();
        }

        public void Activer()
        {
            EstActif = true;
            MarkUpdated();
        }
    }
}
