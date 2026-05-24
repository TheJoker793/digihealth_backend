using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Application.DTOs.Responses
{
    public record PrescriptionResponse
    (
        Guid Id,
    Guid NumeroPrescriptionGuid,
    string NumeroPrescription,
    Guid PatientId,
    Guid MedecinId,
    Guid CabinetId,
    Guid? ConsultationId,
    Guid? DossierId,
    DateOnly Date,
    DateOnly DateExpiration,
    int ValiditeJours,
    int JoursRestants,
    StatutPrescription Statut,
    TypePrescription TypePrescription,
    string? Instructions,
    string? MotifRefus,
    DateTimeOffset? DateValidation,
    List<LigneResponse> Lignes,
    List<InteractionResponse> Interactions
    );
}
