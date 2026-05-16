using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Domain.Entities
{
    public class DocumentMedical : BaseEntity
    {
        public Guid DossierId { get; private set; }
        public DossierMedical? DossierMedical { get; private set; }
        public TypeDocument TypeDocument { get; private set; }
        public string Titre { get; private set; } = default!;
        public string CheminFichier { get; private set; } = default!;
        public DateOnly DateDocument { get; private set; }
        public string MimeType { get; private set; } = default!;
        public long TailleOctets { get; private set; }
        public bool EstSupprime { get; private set; }

        private DocumentMedical() { }

        public static DocumentMedical Create(Guid dossierId, TypeDocument type, string titre, string cheminFichier, string mimeType, long tailleOctets)
        {
            return new DocumentMedical
            {
                DossierId = dossierId,
                TypeDocument = type,
                Titre = titre.Trim(),
                CheminFichier = cheminFichier,
                DateDocument = DateOnly.FromDateTime(DateTime.Today),
                MimeType = mimeType,
                TailleOctets = tailleOctets,
                EstSupprime = false,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public void SoftDelete()
        {
            EstSupprime = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
