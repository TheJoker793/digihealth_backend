using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record DossierSummaryResponse(
        Guid Id,
        string NumeroDossier,
        DateOnly DateOuverture,
        StatutDossier Statut,
        string Motif,
        int NbConsultations
    );
}
