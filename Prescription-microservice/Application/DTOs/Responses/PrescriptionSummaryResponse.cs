using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Application.DTOs.Responses
{
    public record PrescriptionSummaryResponse
    (
        Guid Id,
    string NumeroPrescription,
    Guid PatientId,
    Guid MedecinId,
    DateOnly Date,
    DateOnly DateExpiration,
    StatutPrescription Statut,
    TypePrescription TypePrescription,
    int NombreMedicaments,
    bool HasInteractionBloquante

    );
}
