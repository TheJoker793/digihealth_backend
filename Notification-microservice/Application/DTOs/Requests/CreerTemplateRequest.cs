using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Requests
{
    public record CreerTemplateRequest(
    string Code,
    TypeEvenement TypeEvenement,
    CanalEnvoi Canal,
    string Langue,
    string SujetTemplate,
    string CorpsTemplate,
    string[] Variables
);

}
