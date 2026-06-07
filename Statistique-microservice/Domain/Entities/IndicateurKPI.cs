using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Domain.Entities
{
    /// <summary>
    /// Un indicateur KPI calculé pour un cabinet sur une période donnée.
    /// Entité légère — créée par CalculateurKPIService, jamais modifiée.
    /// </summary>
    public class IndicateurKPI : BaseEntity
    {
        // ═══════════════════════════════════════════
        // IDENTITÉ
        // ═══════════════════════════════════════════
        public Guid CabinetId { get; private set; }
        public Guid? MedecinId { get; private set; }   // null = agrégé cabinet

        // ═══════════════════════════════════════════
        // TYPE ET VALEUR
        // ═══════════════════════════════════════════
        public TypeKPI TypeKPI { get; private set; }
        public string Code { get; private set; } = default!;  // ex: "NB_CONSULTATIONS_MOIS"
        public decimal Valeur { get; private set; }
        public string Unite { get; private set; } = default!;  // "consultations", "%", "TND"…

        // ═══════════════════════════════════════════
        // PÉRIODE (Value Object — stocké via OwnsOne)
        // ═══════════════════════════════════════════
        public PeriodeAnalyse Periode { get; private set; } = default!;

        // ═══════════════════════════════════════════
        // COMPARAISON PÉRIODE PRÉCÉDENTE
        // ═══════════════════════════════════════════
        public decimal? ValeurPrecedente { get; private set; }   // même KPI période N-1

        /// <summary>
        /// Variation en pourcentage par rapport à la période précédente.
        /// Null si pas de données N-1.
        /// </summary>
        public decimal? VariationPourcentage { get; private set; }

        // ═══════════════════════════════════════════
        // FACTORY
        // ═══════════════════════════════════════════
        public static IndicateurKPI Create(
            Guid cabinetId,
            TypeKPI typeKPI,
            string code,
            decimal valeur,
            string unite,
            PeriodeAnalyse periode,
            Guid? medecinId = null,
            decimal? valeurPrecedente = null)
        {
            var kpi = new IndicateurKPI
            {
                CabinetId = cabinetId,
                MedecinId = medecinId,
                TypeKPI = typeKPI,
                Code = code.ToUpperInvariant(),
                Valeur = valeur,
                Unite = unite,
                Periode = periode,
                ValeurPrecedente = valeurPrecedente,
            };

            kpi.VariationPourcentage = kpi.CalculerVariation();
            return kpi;
        }

        // ═══════════════════════════════════════════
        // COMPORTEMENT
        // ═══════════════════════════════════════════

        /// <summary>
        /// Calcule la variation % par rapport à la période précédente.
        /// Retourne null si pas de référence N-1 ou si N-1 = 0.
        /// </summary>
        public decimal? CalculerVariation()
        {
            if (!ValeurPrecedente.HasValue || ValeurPrecedente == 0)
                return null;

            return Math.Round(
                (Valeur - ValeurPrecedente.Value) / Math.Abs(ValeurPrecedente.Value) * 100m,
                2);
        }

        /// <summary>Indique si la variation est positive (croissance).</summary>
        public bool? EstEnHausse()
        {
            if (!VariationPourcentage.HasValue) return null;
            return VariationPourcentage > 0;
        }

        public string FormaterValeur()
            => TypeKPI switch
            {
                TypeKPI.TauxOccupation => $"{Valeur:F1}%",
                TypeKPI.ChiffreAffaires => $"{Valeur:N0} TND",
                TypeKPI.TicketMoyenConsultation => $"{Valeur:N0} TND",
                _ => $"{Valeur:N0} {Unite}"
            };

    }
}