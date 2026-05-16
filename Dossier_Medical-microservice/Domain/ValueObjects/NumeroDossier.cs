using System;
using System.Text.RegularExpressions;

namespace Dossier_Medical_microservice.Domain.ValueObjects
{
    /// <summary>
    /// Value Object représentant un numéro de dossier médical.
    /// Format : DMI-{cabinetId}-{annee}-{sequence}
    /// </summary>
    /// 
    // =========================
    // NUMERO DOSSIER
    // =========================
    public record NumeroDossier
    {
        public string Valeur { get; }

        private NumeroDossier(string valeur) => Valeur = valeur;

        // Format : DMI-{cabinetId:6}-{YYYY}-{seq:0000}
        // Exemple : DMI-CAB001-2026-0042
        public static NumeroDossier Create(string cabinetId, int annee, int sequence)
        {
            var cab = cabinetId.Length > 6
                ? cabinetId[..6].ToUpperInvariant()
                : cabinetId.ToUpperInvariant().PadRight(6, '0');

            return new NumeroDossier($"DMI-{cab}-{annee}-{sequence:D4}");
        }

        public static NumeroDossier FromString(string valeur)
        {
            if (string.IsNullOrWhiteSpace(valeur))
                throw new ArgumentException("Numéro de dossier invalide.");
            return new NumeroDossier(valeur);
        }

        public override string ToString() => Valeur;
    }
}