namespace Document_microservice.Domain.ValueObjects
{
    public sealed class MetadonneeFichier
    {
        public string Auteur { get; }
        public int? NbPages { get; }
        public string Langue { get; }
        public string[] MotsCles { get; }
        public DateTime DateCreationFichier { get; }

        // Constructeur privé — création via factory
        private MetadonneeFichier(
            string auteur,
            int? nbPages,
            string langue,
            string[] motsCles,
            DateTime dateCreationFichier)
        {
            Auteur = auteur;
            NbPages = nbPages;
            Langue = langue;
            MotsCles = motsCles;
            DateCreationFichier = dateCreationFichier;
        }

        public static MetadonneeFichier Create(
        string auteur,
        string langue = "fr",
        int? nbPages = null,
        string[]? motsCles = null,
        DateTime? dateCreationFichier = null)
        {
            return new MetadonneeFichier(
                auteur,
                nbPages,
                langue,
                motsCles ?? [],
                dateCreationFichier ?? DateTime.UtcNow);
        }

        // Value Object : égalité structurelle
        public override bool Equals(object? obj)
        {
            if (obj is not MetadonneeFichier other) return false;
            return Auteur == other.Auteur
                && NbPages == other.NbPages
                && Langue == other.Langue
                && MotsCles.SequenceEqual(other.MotsCles);
        }

        public override int GetHashCode()
            => HashCode.Combine(Auteur, NbPages, Langue);


    }
}
