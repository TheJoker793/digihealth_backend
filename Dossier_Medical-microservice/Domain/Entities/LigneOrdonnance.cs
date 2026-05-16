namespace Dossier_Medical_microservice.Domain.Entities
{
    public class LigneOrdonnance : BaseEntity
    {
        public Guid OrdonnanceId { get; private set; }
        public Ordonnance Ordonnance { get; private set; } = default!;

        public Guid MedicamentId { get; private set; }
        public string NomMedicament { get; private set; } = default!;
        public string Posologie { get; private set; } = default!;
        public int DureeJours { get; private set; }
        public int Quantite { get; private set; }

        private LigneOrdonnance() { }

        public static LigneOrdonnance Create(
            Guid ordonnanceId,
            Guid medicamentId,
            string nomMedicament,
            string posologie,
            int dureeJours,
            int quantite)
        {
            return new LigneOrdonnance
            {
                OrdonnanceId = ordonnanceId,
                MedicamentId = medicamentId,
                NomMedicament = nomMedicament.Trim(),
                Posologie = posologie.Trim(),
                DureeJours = dureeJours,
                Quantite = quantite,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}