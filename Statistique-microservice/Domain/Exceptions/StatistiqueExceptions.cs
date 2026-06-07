namespace Statistique_microservice.Domain.Exceptions
{
    public class RapportIntrouvableException(Guid id)
    : Exception($"Rapport {id} introuvable.");

    public class RapportAnnuleException(Guid id)
        : Exception($"Le rapport {id} est annulé — opération interdite.");

    public class PeriodeInvalideException(string message)
        : Exception($"Période invalide : {message}");

    public class KPIIntrouvableException(string code, Guid cabinetId)
        : Exception($"KPI '{code}' introuvable pour le cabinet {cabinetId}.");

    public class SnapshotDejaConsolideException(Guid cabinetId, string date)
        : Exception($"Le snapshot du cabinet {cabinetId} pour le {date} est déjà consolidé.");

    public class SnapshotIntrouvableException(Guid cabinetId, string date)
        : Exception($"Snapshot introuvable pour le cabinet {cabinetId} à la date {date}.");

    public class AbonnementIntrouvableException(Guid id)
        : Exception($"Abonnement {id} introuvable.");

    public class TableauDeBordIntrouvableException(Guid id)
        : Exception($"Tableau de bord {id} introuvable.");

}
