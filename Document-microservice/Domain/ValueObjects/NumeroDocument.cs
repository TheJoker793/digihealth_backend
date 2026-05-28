namespace Document_microservice.Domain.ValueObjects
{
    public sealed class NumeroDocument
    {
        public string Valeur { get; }

        private NumeroDocument(string valeur) => Valeur = valeur;

        public static NumeroDocument Creer(string cabinetIdCourt, int annee, int sequence)
        {
            if (string.IsNullOrWhiteSpace(cabinetIdCourt))
                throw new ArgumentException("cabinetIdCourt requis.");

            if (sequence <= 0)
                throw new ArgumentOutOfRangeException(nameof(sequence), "La séquence doit être > 0.");

            var valeur = $"DOC-{cabinetIdCourt.ToUpperInvariant()}-{annee}-{sequence:D4}";
            return new NumeroDocument(valeur);
        }

        public static NumeroDocument Parse(string valeur)
        {
            if (string.IsNullOrWhiteSpace(valeur))
                throw new ArgumentException("Numéro vide.");

            return new NumeroDocument(valeur);
        }

        public override string ToString() => Valeur;

        public override bool Equals(object? obj)
            => obj is NumeroDocument other && Valeur == other.Valeur;

        public override int GetHashCode() => Valeur.GetHashCode();
    }
}
