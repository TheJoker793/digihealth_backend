using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record DossierResponse(
        Guid Id,
        string NumeroDossier,
        Guid PatientId,
        Guid MedecinId,
        DateOnly DateOuverture,
        StatutDossier Statut,
        string Motif,
        string? Anamnese,
        int NbConsultations,
        int NbDocuments
    );
}
