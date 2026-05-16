using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Domain.Entities
{
    public class Diagnostic : BaseEntity
    {
        public Guid ConsultationId { get; private set; }
        public Consultation? Consultation { get; private set; }
        public string CodeCIM11 { get; private set; } = default!;
        public string LibelleCIM11 { get; private set; } = default!;
        public TypeDiagnostic Type { get; private set; }
        public string? Commentaire { get; private set; }
        public bool EstConfirme { get; private set; }

        private Diagnostic() { }

        public static Diagnostic Create(Guid consultationId, string codeCim11, string libelle, TypeDiagnostic type, string? commentaire = null)
        {
            return new Diagnostic
            {
                ConsultationId = consultationId,
                CodeCIM11 = codeCim11.Trim().ToUpperInvariant(),
                LibelleCIM11 = libelle.Trim(),
                Type = type,
                Commentaire = commentaire?.Trim(),
                EstConfirme = false,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public void Confirmer()
        {
            EstConfirme = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Modifier(string libelle, string? commentaire)
        {
            LibelleCIM11 = libelle.Trim();
            Commentaire = commentaire?.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
