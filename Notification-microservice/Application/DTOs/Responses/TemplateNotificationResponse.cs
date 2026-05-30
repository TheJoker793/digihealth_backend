using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Responses
{
    public record TemplateNotificationResponse(
    Guid Id,
    string Code,
    TypeEvenement TypeEvenement,
    CanalEnvoi Canal,
    string Langue,
    string SujetTemplate,
    string[] Variables,
    bool EstActif,
    DateTimeOffset CreatedAt
);
}
