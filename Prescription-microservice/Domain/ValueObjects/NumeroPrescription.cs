namespace Prescription_microservice.Domain.ValueObjects
{
    public record NumeroPrescription
    {
        public string Valeur { get; }

        private NumeroPrescription(string valeur) => Valeur = valeur;

        // Format : RX-{cabinetId:6}-{YYYY}-{seq:0000}
        // Exemple : RX-CAB001-2026-0015
        public static NumeroPrescription Create(string cabinetId, int annee, long sequence)
        {
            var cab = cabinetId.Length > 6
                ? cabinetId[..6].ToUpperInvariant()
                : cabinetId.ToUpperInvariant().PadRight(6, '0');

            return new NumeroPrescription($"RX-{cab}-{annee}-{sequence:D4}");
        }

        public static NumeroPrescription FromString(string valeur)
        {
            if (string.IsNullOrWhiteSpace(valeur))
                throw new ArgumentException("Numéro de prescription invalide.");
            return new NumeroPrescription(valeur);
        }

        public override string ToString() => Valeur;
    }
}
