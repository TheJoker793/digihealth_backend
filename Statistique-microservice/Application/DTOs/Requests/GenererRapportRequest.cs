using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record GenererRapportRequest(
    TypeRapport TypeRapport,
    Guid CabinetId,
    TypePeriode TypePeriode,
    string? DateDebut = null,       // "2024-01-01" — requis si TypePeriode = Personnalise
    string? DateFin = null,         // "2024-01-31"
    Guid? MedecinId = null,
    DateTimeOffset? DatePlanifiee = null
);
}
