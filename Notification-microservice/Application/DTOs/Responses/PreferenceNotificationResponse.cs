using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Responses
{
    public record PreferenceNotificationResponse(
    Guid Id,
    Guid DestinataireId,
    string TypeDestinataire,
    CanalEnvoi[] CanauxActifs,
    string? HeureDebut,
    string? HeureFin,
    bool EstOptOut,
    string Langue
);
}
