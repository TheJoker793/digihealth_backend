namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record ExamenCliniqueResponse(
        decimal? Poids,
        decimal? Taille,
        string? TA,
        int? Pouls,
        decimal? Temperature,
        decimal? IMC,
        string? Observations
    );
}
