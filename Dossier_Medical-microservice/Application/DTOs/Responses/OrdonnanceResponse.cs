namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record OrdonnanceResponse(
    Guid Id,
    DateOnly Date,
    int ValiditeJours,
    bool IsExpired,
    string? Instructions,
    IEnumerable<LigneOrdonnanceResponse> Lignes
);

    public record LigneOrdonnanceResponse(
        Guid MedicamentId,
        string NomMedicament,
        string Posologie,
        int DureeJours,
        int Quantite
    );

}
