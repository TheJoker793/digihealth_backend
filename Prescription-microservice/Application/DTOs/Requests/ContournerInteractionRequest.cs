namespace Prescription_microservice.Application.DTOs.Requests
{
    public record ContournerInteractionRequest
    (
        Guid PrescriptionId,
        Guid InteractionId,
        Guid MedecinId,
        string Justification
    );
}
