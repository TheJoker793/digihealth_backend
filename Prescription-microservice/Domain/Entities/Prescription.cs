using Prescription_microservice.Domain.Enums;
using Prescription_microservice.Domain.Events;
using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Domain.Entities
{
    public class Prescription : BaseEntity
    {
        // ── Identité ─────────────────────────────────────────
        public Guid NumeroPrescriptionGuid { get; private set; }
        public NumeroPrescription NumeroPrescription { get; private set; } = default!;
        public Guid PatientId { get; private set; }
        public Guid MedecinId { get; private set; }
        public Guid CabinetId { get; private set; }
        public Guid? ConsultationId { get; private set; }
        public Guid? DossierId { get; private set; }

        // ── Prescription ─────────────────────────────────────
        public DateOnly Date { get; private set; }
        public int ValiditeJours { get; private set; }
        public StatutPrescription Statut { get; private set; } = StatutPrescription.Brouillon;
        public TypePrescription TypePrescription { get; private set; }
        public string? Instructions { get; private set; }
        public string? MotifRefus { get; private set; }
        public DateTimeOffset? DateValidation { get; private set; }

        // ── Navigations ──────────────────────────────────────
        private readonly List<LignePrescription> _lignes = new();
        private readonly List<InteractionDetectee> _interactions = new();

        public IReadOnlyCollection<LignePrescription> Lignes => _lignes.AsReadOnly();
        public IReadOnlyCollection<InteractionDetectee> Interactions => _interactions.AsReadOnly();

        private Prescription() { }

        // ── FACTORY ──────────────────────────────────────────
        public static Prescription Create(
            Guid patientId,
            Guid medecinId,
            Guid cabinetId,
            NumeroPrescription numeroPrescription,
            TypePrescription type,
            int validiteJours,
            Guid? consultationId = null,
            Guid? dossierId = null,
            string? instructions = null)
        {
            if (validiteJours < 1 || validiteJours > 365)
                throw new ArgumentException("La validité doit être entre 1 et 365 jours.");

            var prescription = new Prescription
            {
                PatientId = patientId,
                MedecinId = medecinId,
                CabinetId = cabinetId,
                NumeroPrescription = numeroPrescription,
                TypePrescription = type,
                Date = DateOnly.FromDateTime(DateTime.Today),
                ValiditeJours = validiteJours,
                ConsultationId = consultationId,
                DossierId = dossierId,
                Instructions = instructions?.Trim(),
                Statut = StatutPrescription.Brouillon,
                CreatedAt = DateTimeOffset.UtcNow
            };

            return prescription;
        }

        // ── DOMAINE — gestion des lignes ─────────────────────
        public LignePrescription AddLigne(
            Guid medicamentId,
            string nomMedicament,
            string dci,
            Posologie posologie,
            int dureeJours,
            int quantite,
            bool renouvellement = false,
            int nbRenouvellements = 0,
            string? commentaire = null)
        {
            if (Statut != StatutPrescription.Brouillon)
                throw new InvalidOperationException(
                    "Impossible de modifier une prescription validée ou clôturée.");

            if (_lignes.Any(l => l.MedicamentId == medicamentId))
                throw new InvalidOperationException(
                    $"Le médicament '{nomMedicament}' est déjà présent dans la prescription.");

            var ligne = LignePrescription.Create(
                Id, medicamentId, nomMedicament, dci,
                posologie, dureeJours, quantite,
                renouvellement, nbRenouvellements, commentaire);

            _lignes.Add(ligne);
            return ligne;
        }

        public void RemoveLigne(Guid ligneId)
        {
            if (Statut != StatutPrescription.Brouillon)
                throw new InvalidOperationException(
                    "Impossible de modifier une prescription non brouillon.");

            var ligne = _lignes.FirstOrDefault(l => l.Id == ligneId)
                ?? throw new InvalidOperationException("Ligne introuvable.");

            _lignes.Remove(ligne);
        }

        // ── DOMAINE — interactions ────────────────────────────
        public InteractionDetectee AddInteraction(
            string medicamentA,
            string medicamentB,
            SeveriteInteraction severite,
            string mecanisme,
            string recommandation)
        {
            var interaction = InteractionDetectee.Create(
                Id, medicamentA, medicamentB, severite, mecanisme, recommandation);
            _interactions.Add(interaction);

            AddDomainEvent(new InteractionDetecteeEvent(
                Id, PatientId, medicamentA, medicamentB, severite));

            return interaction;
        }

        public bool HasInteractionsBloquantes()
            => _interactions.Any(i =>
                i.Severite == SeveriteInteraction.ContreIndication && !i.EstContournee);

        // ── DOMAINE — cycle de vie ────────────────────────────
        public void Valider()
        {
            if (Statut != StatutPrescription.Brouillon)
                throw new InvalidOperationException(
                    $"Une prescription en statut '{Statut}' ne peut pas être validée.");

            if (!_lignes.Any())
                throw new InvalidOperationException(
                    "Une prescription sans médicament ne peut pas être validée.");

            if (HasInteractionsBloquantes())
                throw new InvalidOperationException(
                    "Des contre-indications non contournées bloquent la validation. " +
                    "Veuillez les contourner avec une justification médicale.");

            Statut = StatutPrescription.Validee;
            DateValidation = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;

            AddDomainEvent(new PrescriptionValideEvent(
                Id, PatientId, MedecinId, NumeroPrescription.Valeur));
        }

        public void Refuser(string motif)
        {
            if (Statut != StatutPrescription.Brouillon)
                throw new InvalidOperationException(
                    "Seule une prescription en brouillon peut être refusée.");

            if (string.IsNullOrWhiteSpace(motif))
                throw new ArgumentException("Le motif de refus est obligatoire.");

            Statut = StatutPrescription.Refusee;
            MotifRefus = motif.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;

            AddDomainEvent(new PrescriptionRefuseeEvent(Id, PatientId, motif));
        }

        public void Annuler()
        {
            if (Statut is StatutPrescription.Annulee or StatutPrescription.Expiree)
                throw new InvalidOperationException("Cette prescription est déjà annulée ou expirée.");

            Statut = StatutPrescription.Annulee;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void MarquerExpiree()
        {
            if (Statut != StatutPrescription.Validee)
                return;

            Statut = StatutPrescription.Expiree;
            UpdatedAt = DateTimeOffset.UtcNow;

            AddDomainEvent(new PrescriptionExpireEvent(Id, PatientId));
        }

        // ── HELPERS ──────────────────────────────────────────
        public bool IsExpired()
            => Statut == StatutPrescription.Validee
            && DateOnly.FromDateTime(DateTime.Today) > Date.AddDays(ValiditeJours);

        public DateOnly DateExpiration => Date.AddDays(ValiditeJours);

        public int JoursRestants()
        {
            var restants = DateExpiration.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;
            return Math.Max(0, restants);
        }
    }
}
