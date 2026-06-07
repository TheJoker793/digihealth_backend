using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record MettreAJourAbonnementRequest(
    FrequenceRapport? Frequence = null,
    string[]? Destinataires = null
);
}
