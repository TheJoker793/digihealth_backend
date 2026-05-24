using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Application.DTOs.Responses
{
    public record InteractionResponse
    (
        Guid Id,
    string MedicamentA,
    string MedicamentB,
    SeveriteInteraction Severite,
    string Mecanisme,
    string Recommandation,
    bool EstContournee,
    string? Justification,
    Guid? ContourneePar,
    DateTimeOffset? ContourneeAt
    );
}
