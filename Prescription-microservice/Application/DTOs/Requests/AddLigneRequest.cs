namespace Prescription_microservice.Application.DTOs.Requests
{
    public record AddLigneRequest
    (
        Guid PrescriptionId,
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
