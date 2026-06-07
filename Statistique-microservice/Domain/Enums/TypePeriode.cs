namespace Statistique_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // TypePeriode — granularité temporelle
    // ═══════════════════════════════════════════════════════════
    public enum TypePeriode
    {
        Journee = 1,
        Hebdomadaire = 2,
        Mensuel = 3,
        Trimestriel = 4,
        Annuel = 5,
        Personnalise = 99,
    }
}
