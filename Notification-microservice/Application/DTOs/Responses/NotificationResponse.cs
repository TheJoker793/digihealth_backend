using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Responses
{
    public record NotificationResponse(
    Guid Id,
    string Numero,
    TypeEvenement TypeEvenement,
    Guid DestinataireId,
    string TypeDestinataire,
    CanalEnvoi Canal,
    string Sujet,
    StatutNotification Statut,
    int NbTentatives,
    int MaxTentatives,
    DateTimeOffset? DateEnvoi,
    DateTimeOffset? DateProgrammee,
    string? DerniereErreur,
    DateTimeOffset CreatedAt,
    IEnumerable<HistoriqueEnvoiResponse>? Historiques = null
);
}
