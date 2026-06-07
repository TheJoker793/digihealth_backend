using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Responses
{
    public record AbonnementResponse(
    Guid Id,
    Guid CabinetId,
    TypeRapport TypeRapport,
    FrequenceRapport Frequence,
    string[] Destinataires,
    bool EstActif,
    DateTimeOffset? DernierEnvoi,
    DateTimeOffset? ProchainEnvoi,
    DateTimeOffset CreatedAt
);
}
