using System;

namespace Dossier_Medical_microservice.Domain.ValueObjects
{

    // =========================
    // EXAMEN CLINIQUE (owned by Consultation)
    // =========================
    public record ExamenClinique(
        decimal? Poids,
        decimal? Taille,
        string? TA,
        int? Pouls,
        decimal? Temperature,
        string? Observations
    )
    {
        public decimal? IMC()
        {
            if (!Poids.HasValue || !Taille.HasValue || Taille == 0) return null;
            var tailleM = Taille.Value / 100m;
            return Math.Round(Poids.Value / (tailleM * tailleM), 1);
        }

        public static ExamenClinique Empty()
            => new(null, null, null, null, null, null);

        public static ExamenClinique Create(
            decimal? poids = null,
            decimal? taille = null,
            string? ta = null,
            int? pouls = null,
            decimal? temperature = null,
            string? observations = null)
            => new(poids, taille, ta, pouls, temperature, observations);
    }

    

    }
