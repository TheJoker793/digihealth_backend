namespace Notification_microservice.Application.DTOs.Requests
{
    public record MettreAJourTemplateRequest(
    string SujetTemplate,
    string CorpsTemplate,
    string[] Variables
);
}
