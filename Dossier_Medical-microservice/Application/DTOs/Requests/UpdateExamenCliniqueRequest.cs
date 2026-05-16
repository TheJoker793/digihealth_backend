using Dossier_Medical_microservice.Domain.ValueObjects;

namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record UpdateExamenCliniqueRequest
    
    (
        decimal? Poids,
        decimal? Taille,
        string? TA,
        int? Pouls,
        decimal? Temperature,
        string? Observations
    );
}
