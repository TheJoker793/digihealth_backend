namespace Notification_microservice.Domain.Enums
{
    // ═══════════════════════════════════════════════════════════
    // ResultatEnvoi — résultat d'une tentative dans HistoriqueEnvoi
    // ═══════════════════════════════════════════════════════════
    public enum ResultatEnvoi
    {
        Succes = 1,
        Echec = 2,
        EchecProvider = 3,   // provider externe indisponible (SendGrid down…)
        OptOut = 4,   // destinataire a refusé ce canal
        HorsPlage = 5,   // hors plage horaire → reporté
    }
}
