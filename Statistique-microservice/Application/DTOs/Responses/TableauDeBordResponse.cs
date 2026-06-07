using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Responses
{
    public record TableauDeBordResponse(
    Guid Id,
    Guid CabinetId,
    Guid ProprietaireId,
    string Nom,
    TypeKPI[] KPIsAffiches,
    TypePeriode PeriodeDefaut,
    int NbSemainesTendance,
    bool EstParDefaut,
    DateTimeOffset CreatedAt
);

}
