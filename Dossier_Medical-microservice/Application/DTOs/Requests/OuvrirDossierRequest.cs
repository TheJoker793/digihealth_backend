namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record OuvrirDossierRequest
        (
        Guid PatientId,
        Guid MedecinId,
        Guid CabinetId,
        string Motif,
        string? Anamnese = null

        );
    






    
}
