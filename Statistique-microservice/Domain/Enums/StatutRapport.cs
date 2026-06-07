namespace Statistique_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // StatutRapport — cycle de vie
    // ═══════════════════════════════════════════════════════════
    public enum StatutRapport
    {
        EnAttente = 1,
        Planifie = 2,
        EnCours = 3,
        Genere = 4,
        Echoue = 5,
        Annule = 6,
    }
}
