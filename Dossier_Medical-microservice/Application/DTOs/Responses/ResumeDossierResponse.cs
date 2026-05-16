namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record ResumeDossierResponse(
        Guid DossierId,
        string NumeroDossier,
        int NbConsultations,
        IEnumerable<string> DerniersCodesCIM11,
        IEnumerable<OrdonnanceResponse> OrdonnancesActives,
        DateTimeOffset GeneratedAt
    );
}
