using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Requests
{
    public record MettreAJourPreferenceRequest(
    CanalEnvoi[] CanauxActifs,
    string? HeureDebut = null,          // "08:00"
    string? HeureFin = null,            // "20:00"
    string Langue = "fr"
);
}
