using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Responses
{
    public record RapportResponse(
    Guid Id,
    string Numero,
    TypeRapport TypeRapport,
    Guid CabinetId,
    Guid? MedecinId,
    PeriodeResponse Periode,
    StatutRapport Statut,
    DateTimeOffset? DateGeneration,
    DateTimeOffset? DatePlanifiee,
    string? MessageErreur,
    bool AExportPdf,
    bool AExportExcel,
    IEnumerable<KPIResponse>? Indicateurs,
    DateTimeOffset CreatedAt
);
}
