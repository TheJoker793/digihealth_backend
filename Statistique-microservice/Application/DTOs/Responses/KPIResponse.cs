using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.DTOs.Responses
{
    public record KPIResponse(
    Guid Id,
    TypeKPI TypeKPI,
    string Code,
    decimal Valeur,
    string Unite,
    string ValeurFormatee,
    decimal? ValeurPrecedente,
    decimal? VariationPourcentage,
    bool? EstEnHausse,
    PeriodeResponse Periode
);
}
