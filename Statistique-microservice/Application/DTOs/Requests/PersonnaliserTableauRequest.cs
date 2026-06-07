using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record PersonnaliserTableauRequest(
    string Nom,
    TypeKPI[] KPIsAffiches,
    TypePeriode PeriodeDefaut,
    int NbSemainesTendance = 12
);
}
