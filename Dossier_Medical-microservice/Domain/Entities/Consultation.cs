using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Events;
using Dossier_Medical_microservice.Domain.ValueObjects;

namespace Dossier_Medical_microservice.Domain.Entities
{
    public class Consultation : BaseEntity
    {
        public Guid DossierId { get; private set; }
        public DossierMedical? DossierMedical { get; set; }
        public Guid? RendezVousId { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public TypeConsultation TypeConsultation { get; private set; }
        public string Motif { get; private set; } = default!;
        public StatutConsultation Statut { get; private set; } = StatutConsultation.EnCours;
        public ExamenClinique ExamenClinique { get; private set; } = ExamenClinique.Empty();
        public string? Conclusion { get; private set; }
        public DateTimeOffset? DateCloture { get; private set; }

        private readonly List<Diagnostic> _diagnostics = new();
        private readonly List<Ordonnance> _ordonnances = new();

        public IReadOnlyCollection<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();
        public IReadOnlyCollection<Ordonnance> Ordonnances => _ordonnances.AsReadOnly();

        private Consultation() { }

        public static Consultation Create(Guid dossierId, Guid? rendezVousId, TypeConsultation type, string motif)
        {
            var c = new Consultation
            {
                DossierId = dossierId,
                RendezVousId = rendezVousId,
                Date = DateTimeOffset.UtcNow,
                TypeConsultation = type,
                Motif = motif.Trim(),
                Statut = StatutConsultation.EnCours,
                CreatedAt = DateTimeOffset.UtcNow
            };
            c.AddDomainEvent(new ConsultationCreeeEvent(c.Id, dossierId));
            return c;
        }

        public void UpdateExamenClinique(ExamenClinique examen)
        {
            if (Statut != StatutConsultation.EnCours)
                throw new InvalidOperationException("La consultation est déjà clôturée.");

            ExamenClinique = examen;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Cloturer(string conclusion)
        {
            if (Statut == StatutConsultation.Cloturee)
                throw new InvalidOperationException("La consultation est déjà clôturée.");
            if (string.IsNullOrWhiteSpace(conclusion))
                throw new ArgumentException("La conclusion est obligatoire pour clôturer.");

            Statut = StatutConsultation.Cloturee;
            Conclusion = conclusion.Trim();
            DateCloture = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
            AddDomainEvent(new ConsultationClotureEvent(Id, DossierId));
        }

        public Diagnostic AddDiagnostic(string codeCim11, string libelle, TypeDiagnostic type, string? commentaire = null)
        {
            if (Statut != StatutConsultation.EnCours)
                throw new InvalidOperationException("Impossible d'ajouter un diagnostic à une consultation clôturée.");

            var d = Diagnostic.Create(Id, codeCim11, libelle, type, commentaire);
            _diagnostics.Add(d);
            return d;
        }

        public Ordonnance AddOrdonnance(int validiteJours, string? instructions = null)
        {
            if (Statut != StatutConsultation.EnCours)
                throw new InvalidOperationException("Impossible de créer une ordonnance sur une consultation clôturée.");

            var o = Ordonnance.Create(Id, validiteJours, instructions);
            _ordonnances.Add(o);
            AddDomainEvent(new OrdonnanceCreeeEvent(o.Id, Id, DossierId));
            return o;
        }
    }
}
