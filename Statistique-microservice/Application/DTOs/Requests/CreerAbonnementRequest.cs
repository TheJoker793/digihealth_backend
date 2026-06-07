using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record CreerAbonnementRequest(
    Guid CabinetId,
    Guid CreePar,
    TypeRapport TypeRapport,
    FrequenceRapport Frequence,
    string[] Destinataires
);
}
