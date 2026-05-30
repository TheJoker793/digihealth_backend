using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Responses
{
    public record HistoriqueEnvoiResponse(
    Guid Id,
    CanalEnvoi Canal,
    ResultatEnvoi Resultat,
    string? MessageErreur,
    string? ProviderResponse,
    long DureeMs,
    DateTimeOffset DateTentative
);
}
