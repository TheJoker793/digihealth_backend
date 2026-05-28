using Document_microservice.Domain.Enums;
using Document_microservice.Domain.ValueObjects;

namespace Document_microservice.Domain.Entities
{
    public class VersionDocument : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public Guid DocumentMedicalId { get; private set; }
        public int NumeroVersion { get; private set; }
        public bool EstActive { get; private set; }

        // ═══════════════════════════════════════════
        // FICHIER
        // ═══════════════════════════════════════════
        public string CheminFichier { get; private set; } = default!;   // ex: cabinet-1/patients/456/CR-2024-001/v3.pdf
        public string NomFichier { get; private set; } = default!;
        public FormatFichier Format { get; private set; }
        public long TailleOctets { get; private set; }
        public string ChecksumSha256 { get; private set; } = default!;  // intégrité fichier

        // ═══════════════════════════════════════════
        // MÉTADONNÉES (Value Object — colonnes inline via EF OwnsOne)
        // ═══════════════════════════════════════════
        public MetadonneeFichier Metadonnees { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // NAVIGATION EF Core
        // ═══════════════════════════════════════════
        public DocumentMedical DocumentMedical { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static VersionDocument Create(
            Guid documentMedicalId,
            int numeroVersion,
            string cheminFichier,
            string nomFichier,
            FormatFichier format,
            long tailleOctets,
            string checksumSha256,
            MetadonneeFichier metadonnees)
        {
            return new VersionDocument
            {
                DocumentMedicalId = documentMedicalId,
                NumeroVersion = numeroVersion,
                CheminFichier = cheminFichier,
                NomFichier = nomFichier,
                Format = format,
                TailleOctets = tailleOctets,
                ChecksumSha256 = checksumSha256,
                Metadonnees = metadonnees,
                EstActive = true,
            };
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════
        public void Desactiver()
        {
            EstActive = false;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Activer()
        {
            EstActive = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public string GenererCleStockage()
            => CheminFichier;  // clé MinIO directe

        public bool EstExpire(TimeSpan dureeRetention)
            => CreatedAt.Add(dureeRetention) < DateTimeOffset.UtcNow;
    }
}
