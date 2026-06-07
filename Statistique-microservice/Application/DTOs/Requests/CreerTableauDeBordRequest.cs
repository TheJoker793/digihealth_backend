using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Requests
{
    public record CreerTableauDeBordRequest(
    Guid CabinetId,
    Guid ProprietaireId,
    string Nom,
    TypeKPI[]? KPIsAffiches = null,
    TypePeriode PeriodeDefaut = TypePeriode.Mensuel,
    bool EstParDefaut = false
);
}
