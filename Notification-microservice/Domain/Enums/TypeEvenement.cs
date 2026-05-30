namespace Notification_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // TypeEvenement — origine de la notification
    // Chaque valeur correspond à un Consumer RabbitMQ distinct.
    // ═══════════════════════════════════════════════════════════
    public enum TypeEvenement
    {
        // ── Rendez-vous ──────────────────────────────────────────
        RdvConfirme = 1,
        RdvRappel24h = 2,   // rappel J-1
        RdvRappel2h = 3,   // rappel H-2
        RdvAnnule = 4,
        RdvModifie = 5,

        // ── Documents médicaux ───────────────────────────────────
        DocumentPublie = 10,
        OrdonnanceSignee = 11,
        ResultatsBiologie = 12,
        CompteRenduPret = 13,

        // ── Compte utilisateur ───────────────────────────────────
        BienvenuePlateforme = 20,
        ReinitialisationMdp = 21,
        Verification2FA = 22,

        // ── Facturation ──────────────────────────────────────────
        FactureGeneree = 30,
        PaiementRecu = 31,
        PaiementEchoue = 32,
    }
}
