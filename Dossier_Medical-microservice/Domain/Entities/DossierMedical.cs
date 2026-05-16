
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Events;
using Dossier_Medical_microservice.Domain.ValueObjects;

namespace Dossier_Medical_microservice.Domain.Entities
{
    public class DossierMedical : BaseEntity
    {
        public Guid PatientId { get; private set; }
        public Guid MedecinId { get; private set; }
        public Guid CabinetId { get; private set; }
        public NumeroDossier NumeroDossier { get; private set; } = default!;
        public DateOnly DateOuverture { get; private set; }
        public StatutDossier Statut { get; private set; } = StatutDossier.Ouvert;
        public string Motif { get; private set; } = default!;
        public string? Anamnese { get; private set; }
        public DateOnly? DateCloture { get; private set; }

        private readonly List<Consultation> _consultations = new();
        private readonly List<DocumentMedical> _documents = new();

        public IReadOnlyCollection<Consultation> Consultations => _consultations.AsReadOnly();
        public IReadOnlyCollection<DocumentMedical> Documents => _documents.AsReadOnly();

        private DossierMedical() { }

        public static DossierMedical Create(Guid patientId, Guid medecinId, Guid cabinetId, NumeroDossier numeroDossier, string motif, string? anamnese = null)
        {
            if (string.IsNullOrWhiteSpace(motif))
                throw new ArgumentException("Le motif est obligatoire.");

            var dossier = new DossierMedical
            {
                PatientId = patientId,
                MedecinId = medecinId,
                CabinetId = cabinetId,
                NumeroDossier = numeroDossier,
                DateOuverture = DateOnly.FromDateTime(DateTime.Today),
                Motif = motif.Trim(),
                Anamnese = anamnese?.Trim(),
                Statut = StatutDossier.Ouvert,
                CreatedAt = DateTimeOffset.UtcNow
            };
            dossier.AddDomainEvent(new DossierOuvertEvent(dossier.Id, patientId, medecinId));
            return dossier;
        }

        public void Cloturer()
        {
            if (Statut == StatutDossier.Cloture)
                throw new InvalidOperationException("Le dossier est déjà clôturé.");

            Statut = StatutDossier.Cloture;
            DateCloture = DateOnly.FromDateTime(DateTime.Today);
            UpdatedAt = DateTimeOffset.UtcNow;
            AddDomainEvent(new DossierClotureEvent(Id, PatientId));
        }

        public void Archiver()
        {
            if (Statut != StatutDossier.Cloture)
                throw new InvalidOperationException("Seul un dossier clôturé peut être archivé.");

            Statut = StatutDossier.Archive;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateAnamnese(string anamnese)
        {
            Anamnese = anamnese.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Consultation AddConsultation(Guid rendezVousId, TypeConsultation type, string motif)
        {
            if (Statut != StatutDossier.Ouvert)
                throw new InvalidOperationException("Impossible d'ajouter une consultation à un dossier non ouvert.");

            var c = Consultation.Create(Id, rendezVousId, type, motif);
            _consultations.Add(c);
            return c;
        }

        public DocumentMedical AddDocument(TypeDocument typeDoc, string titre, string cheminFichier, string mimeType, long tailleOctets)
        {
            var doc = DocumentMedical.Create(Id, typeDoc, titre, cheminFichier, mimeType, tailleOctets);
            _documents.Add(doc);
            return doc;
        }
    }
}
