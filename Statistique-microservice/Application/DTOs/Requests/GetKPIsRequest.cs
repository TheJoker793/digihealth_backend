using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record GetKPIsRequest(
    Guid CabinetId,
    TypePeriode TypePeriode,
    string? DateDebut = null,
    string? DateFin = null,
    TypeKPI[]? Types = null,        // null = tous les KPIs
    Guid? MedecinId = null
);
}
