namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record CreateOrdonnanceRequest(
        int ValiditeJours,
        string? Instructions,
        IEnumerable<LigneRequest> Lignes
    );
}
