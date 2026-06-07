using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Domain.ValueObjects
{
    /// <summary>
    /// Value Object immuable représentant une période d'analyse.
    /// Stocké via EF Core OwnsOne (colonnes inline, pas de table séparée).
    /// </summary>
    public sealed class PeriodeAnalyse
    {
        public DateOnly DateDebut { get; }
        public DateOnly DateFin { get; }
        public TypePeriode TypePeriode { get; }

        private PeriodeAnalyse(DateOnly dateDebut, DateOnly dateFin, TypePeriode type)
        {
            if (dateFin < dateDebut)
                throw new ArgumentException("DateFin doit être >= DateDebut.");

            DateDebut = dateDebut;
            DateFin = dateFin;
            TypePeriode = type;
        }

        // ═══════════════════════════════════════════
        // FACTORIES STANDARDS
        // ═══════════════════════════════════════════

        public static PeriodeAnalyse Personnalisee(DateOnly debut, DateOnly fin)
            => new(debut, fin, TypePeriode.Personnalise);

        public static PeriodeAnalyse Journee(DateOnly date)
            => new(date, date, TypePeriode.Journee);

        public static PeriodeAnalyse SemaineCourante()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var lundi = today.AddDays(-(int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);
            return new(lundi, lundi.AddDays(6), TypePeriode.Hebdomadaire);
        }

        public static PeriodeAnalyse MoisCourant()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var debut = new DateOnly(today.Year, today.Month, 1);
            var fin = debut.AddMonths(1).AddDays(-1);
            return new(debut, fin, TypePeriode.Mensuel);
        }

        public static PeriodeAnalyse MoisPrecedent()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var debut = new DateOnly(today.Year, today.Month, 1).AddMonths(-1);
            var fin = debut.AddMonths(1).AddDays(-1);
            return new(debut, fin, TypePeriode.Mensuel);
        }

        public static PeriodeAnalyse AnneeCourante()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return new(
                new DateOnly(today.Year, 1, 1),
                new DateOnly(today.Year, 12, 31),
                TypePeriode.Annuel);
        }

        public static PeriodeAnalyse AnneePrecedente()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var annee = today.Year - 1;
            return new(
                new DateOnly(annee, 1, 1),
                new DateOnly(annee, 12, 31),
                TypePeriode.Annuel);
        }

        /// <summary>Crée la période de type TypePeriode depuis une date de référence.</summary>
        public static PeriodeAnalyse Depuis(DateOnly reference, TypePeriode type)
            => type switch
            {
                TypePeriode.Journee => Journee(reference),
                TypePeriode.Hebdomadaire => SemaineCourante(),
                TypePeriode.Mensuel => MoisCourant(),
                TypePeriode.Annuel => AnneeCourante(),
                _ => throw new ArgumentException($"Type {type} non supporté ici.")
            };

        // ═══════════════════════════════════════════
        // OPÉRATIONS
        // ═══════════════════════════════════════════

        /// <summary>Nombre de jours dans la période (inclus).</summary>
        public int NbJours() => DateFin.DayNumber - DateDebut.DayNumber + 1;

        /// <summary>Retourne la période N-1 équivalente (même durée, décalée).</summary>
        public PeriodeAnalyse PeriodePrecedente()
            => TypePeriode switch
            {
                TypePeriode.Journee =>
                    new PeriodeAnalyse(DateDebut.AddDays(-1), DateFin.AddDays(-1), TypePeriode),

                TypePeriode.Hebdomadaire =>
                    new PeriodeAnalyse(DateDebut.AddDays(-7), DateFin.AddDays(-7), TypePeriode),

                TypePeriode.Mensuel =>
                    new PeriodeAnalyse(
                        DateDebut.AddMonths(-1),
                        DateDebut.AddDays(-1),
                        TypePeriode),

                TypePeriode.Annuel =>
                    new PeriodeAnalyse(
                        new DateOnly(DateDebut.Year - 1, 1, 1),
                        new DateOnly(DateDebut.Year - 1, 12, 31),
                        TypePeriode),

                _ =>
                    new PeriodeAnalyse(
                        DateDebut.AddDays(-NbJours()),
                        DateDebut.AddDays(-1),
                        TypePeriode),
            };

        /// <summary>Vérifie si deux périodes se chevauchent.</summary>
        public bool Chevauche(PeriodeAnalyse autre)
            => DateDebut <= autre.DateFin && DateFin >= autre.DateDebut;

        /// <summary>Vérifie si une date est dans la période.</summary>
        public bool Contient(DateOnly date)
            => date >= DateDebut && date <= DateFin;

        // ═══════════════════════════════════════════
        // ÉGALITÉ (Value Object)
        // ═══════════════════════════════════════════
        public override bool Equals(object? obj)
            => obj is PeriodeAnalyse other
               && DateDebut == other.DateDebut
               && DateFin == other.DateFin;

        public override int GetHashCode()
            => HashCode.Combine(DateDebut, DateFin);

        public override string ToString()
            => $"{DateDebut:dd/MM/yyyy} → {DateFin:dd/MM/yyyy} ({TypePeriode})";
    }
}
