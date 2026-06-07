namespace Statistique_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // TypeRapport — catégorie du rapport généré
    // ═══════════════════════════════════════════════════════════
    public enum TypeRapport
    {
        ActiviteCabinet = 1,   // vue globale : consultations, RDV, patients
        Patientologie = 2,   // démographie, pathologies fréquentes, répartition âge/sexe
        Financier = 3,   // CA, impayés, ticket moyen, évolution mensuelle
        Prescriptions = 4,   // médicaments fréquents, interactions, renouvellements
        RendezVous = 5,   // taux occupation, annulations, pics d'affluence
        Performance = 6,   // délai moyen consultation, satisfaction, NPS
    }
}
