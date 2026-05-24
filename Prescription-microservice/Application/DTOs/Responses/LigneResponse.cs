namespace Prescription_microservice.Application.DTOs.Responses
{
    public record LigneResponse
    (
    Guid Id,
    Guid MedicamentId,
    string NomMedicament,
    string DCI,
    string Posologie,
    int DureeJours,
    int Quantite,
    bool Renouvellement,
    int NbRenouvellements,
    string? Commentaire

    );
}
