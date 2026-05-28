namespace Document_microservice.Domain.Entities
{
    public class PartageDocument:BaseEntity
    {
        // ═══════════════════════════════════════════
        // RELATIONS
        // ═══════════════════════════════════════════
        public Guid DocumentMedicalId { get; private set; }
        public DocumentMedical DocumentMedical { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // DESTINATAIRE
        // ═══════════════════════════════════════════
        public Guid DestinataireId { get; private set; }        // MedecinId ou PatientId
        public string TypeDestinataire { get; private set; } = default!; // "Medecin" | "Patient" | "Externe"

        // ═══════════════════════════════════════════
        // ACCÈS
        // ═══════════════════════════════════════════
        public string TokenAcces { get; private set; } = default!;  // GUID opaque
        public bool LectureSeule { get; private set; }
        public int NombreAcces { get; private set; }
        public int? LimiteAcces { get; private set; }               // null = illimité
        public DateTime? DateExpiration { get; private set; }
        public bool EstRevoque { get; private set; }

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static PartageDocument Create(
            Guid documentMedicalId,
            Guid destinataireId,
            string typeDestinataire,
            bool lectureSeule = true,
            DateTime? dateExpiration = null,
            int? limiteAcces = null)
        {
            return new PartageDocument
            {
                DocumentMedicalId = documentMedicalId,
                DestinataireId = destinataireId,
                TypeDestinataire = typeDestinataire,
                TokenAcces = Guid.NewGuid().ToString("N"),   // token opaque 32 chars
                LectureSeule = lectureSeule,
                NombreAcces = 0,
                LimiteAcces = limiteAcces,
                DateExpiration = dateExpiration,
                EstRevoque = false,
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════
        public bool EstValide()
        {
            if (EstRevoque) return false;
            if (DateExpiration.HasValue && DateTime.UtcNow > DateExpiration.Value) return false;
            if (LimiteAcces.HasValue && NombreAcces >= LimiteAcces.Value) return false;
            return true;
        }

        public void EnregistrerAcces()
        {
            if (!EstValide())
                throw new UnauthorizedAccessException("Ce partage est expiré ou révoqué.");

            NombreAcces++;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Revoquer()
        {
            EstRevoque = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
