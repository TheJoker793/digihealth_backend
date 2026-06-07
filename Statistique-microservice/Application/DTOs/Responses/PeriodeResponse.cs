using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Responses
{
    public record PeriodeResponse(
    string DateDebut,
    string DateFin,
    TypePeriode TypePeriode,
    int NbJours
);
}
