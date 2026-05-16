namespace Dossier_Medical_microservice.Domain.Entities
{
    public class Ordonnance : BaseEntity
    {
        public Guid ConsultationId { get; private set; }
        public Consultation? Consultation { get; private set; } 
        public DateOnly Date { get; private set; }
        public int ValiditeJours { get; private set; }
        public string? Instructions { get; private set; }

        private readonly List<LigneOrdonnance> _lignes = new();
        public IReadOnlyCollection<LigneOrdonnance> Lignes => _lignes.AsReadOnly();

        private Ordonnance() { }

        public static Ordonnance Create(Guid consultationId, int validiteJours, string? instructions = null)
        {
            if (validiteJours < 1 || validiteJours > 365)
                throw new ArgumentException("La validité doit être entre 1 et 365 jours.");

            return new Ordonnance
            {
                ConsultationId = consultationId,
                Date = DateOnly.FromDateTime(DateTime.Today),
                ValiditeJours = validiteJours,
                Instructions = instructions?.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public LigneOrdonnance AddLigne(Guid medicamentId, string nomMedicament, string posologie, int dureeJours, int quantite)
        {
            var ligne = LigneOrdonnance.Create(Id, medicamentId, nomMedicament, posologie, dureeJours, quantite);
            _lignes.Add(ligne);
            return ligne;
        }

        public bool IsExpired()
            => DateOnly.FromDateTime(DateTime.Today) > Date.AddDays(ValiditeJours);
    }
}
