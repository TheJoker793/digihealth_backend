namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record LigneRequest(
        Guid MedicamentId,
        string NomMedicament,
        string Posologie,
        int DureeJours,
        int Quantite
    );
}
