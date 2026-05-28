using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Events;
using Document_microservice.Domain.ValueObjects;

namespace Document_microservice.Domain.Entities
{
    public class DocumentMedical : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public string Numero { get; private set; } = default!;
        public string Titre { get; private set; } = default!;
        public string? Description { get; private set; }

        // ═══════════════════════════════════════════
        // TYPE ET STATUT
        // ═══════════════════════════════════════════
        public TypeDocument TypeDocument { get; private set; }
        public StatutDocument Statut { get; private set; }
        public bool EstArchive { get; private set; }

        // ═══════════════════════════════════════════
        // LIAISONS INTER-MICROSERVICES (IDs seulement)
        // ═══════════════════════════════════════════
        public Guid PatientId { get; private set; }
        public Guid MedecinId { get; private set; }
        public Guid CabinetId { get; private set; }
        public Guid? ConsultationId { get; private set; }
        public Guid? PrescriptionId { get; private set; }

        // ═══════════════════════════════════════════
        // VERSIONS (composition)
        // ═══════════════════════════════════════════
        private readonly List<VersionDocument> _versions = new();
        public IReadOnlyCollection<VersionDocument> Versions => _versions.AsReadOnly();

        public VersionDocument? VersionActive =>
            _versions.OrderByDescending(v => v.NumeroVersion).FirstOrDefault(v => v.EstActive);

        // ═══════════════════════════════════════════
        // PARTAGES (composition)
        // ═══════════════════════════════════════════
        private readonly List<PartageDocument> _partages = new();
        public IReadOnlyCollection<PartageDocument> Partages => _partages.AsReadOnly();

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static DocumentMedical Create(
            string numero,
            string titre,
            TypeDocument type,
            Guid patientId,
            Guid medecinId,
            Guid cabinetId,
            Guid? consultationId = null,
            Guid? prescriptionId = null,
            string? description = null)
        {
            var doc = new DocumentMedical
            {
                Numero = numero,
                Titre = titre.Trim(),
                Description = description,
                TypeDocument = type,
                Statut = StatutDocument.Brouillon,
                EstArchive = false,
                PatientId = patientId,
                MedecinId = medecinId,
                CabinetId = cabinetId,
                ConsultationId = consultationId,
                PrescriptionId = prescriptionId,
            };

            doc.AddDomainEvent(new DocumentCreeEvent(doc.Id, patientId, medecinId, type));
            return doc;
        }

        // ═══════════════════════════════════════════
        // GESTION DES VERSIONS
        // ═══════════════════════════════════════════
        public VersionDocument AjouterVersion(
            string cheminFichier,
            string nomFichier,
            FormatFichier format,
            long tailleOctets,
            string checksumSha256,
            MetadonneeFichier metadonnees)
        {
            // Désactiver l'ancienne version active
            foreach (var v in _versions.Where(v => v.EstActive))
                v.Desactiver();

            var numeroVersion = _versions.Count + 1;

            var version = VersionDocument.Create(
                Id, numeroVersion, cheminFichier, nomFichier,
                format, tailleOctets, checksumSha256, metadonnees);

            _versions.Add(version);
            MarkUpdated();
            return version;
        }

        public void RevenirAVersion(int numeroVersion)
        {
            var cible = _versions.FirstOrDefault(v => v.NumeroVersion == numeroVersion)
                ?? throw new InvalidOperationException($"Version {numeroVersion} introuvable.");

            foreach (var v in _versions.Where(v => v.EstActive))
                v.Desactiver();

            cible.Activer();
            MarkUpdated();
        }

        // ═══════════════════════════════════════════
        // CYCLE DE VIE DU DOCUMENT
        // ═══════════════════════════════════════════
        public void Publier()
        {
            if (Statut == StatutDocument.Archive)
                throw new InvalidOperationException("Impossible de publier un document archivé.");

            if (VersionActive is null)
                throw new InvalidOperationException("Impossible de publier un document sans version.");

            Statut = StatutDocument.Publie;
            MarkUpdated();
            AddDomainEvent(new DocumentPublieEvent(Id, PatientId, TypeDocument));
        }

        public void Signer()
        {
            if (Statut != StatutDocument.Publie)
                throw new InvalidOperationException("Seul un document publié peut être signé.");

            Statut = StatutDocument.Signee;
            MarkUpdated();
        }

        public void Archiver()
        {
            if (EstArchive)
                return;

            EstArchive = true;
            Statut = StatutDocument.Archive;
            MarkUpdated();
            AddDomainEvent(new DocumentArchiveEvent(Id, PatientId));
        }

        public void ModifierTitre(string nouveauTitre)
        {
            if (Statut == StatutDocument.Archive)
                throw new InvalidOperationException("Impossible de modifier un document archivé.");

            Titre = nouveauTitre.Trim();
            MarkUpdated();
        }

        // ═══════════════════════════════════════════
        // PARTAGE
        // ═══════════════════════════════════════════
        public PartageDocument Partager(
            Guid destinataireId,
            string typeDestinataire,
            bool lectureSeule = true,
            DateTime? dateExpiration = null)
        {
            if (EstArchive)
                throw new InvalidOperationException("Impossible de partager un document archivé.");

            var partage = PartageDocument.Create(Id, destinataireId, typeDestinataire, lectureSeule, dateExpiration);
            _partages.Add(partage);
            MarkUpdated();

            AddDomainEvent(new DocumentPartageEvent(Id, PatientId, destinataireId));
            return partage;
        }

        public void RevoquerPartage(Guid partageId)
        {
            var partage = _partages.FirstOrDefault(p => p.Id == partageId)
                ?? throw new KeyNotFoundException("Partage introuvable.");

            partage.Revoquer();
            MarkUpdated();
        }




    }
}
