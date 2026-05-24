namespace Prescription_microservice.Application.DTOs.Requests
{
    public record RefuserRequest
    (
    Guid PrescriptionId,
    string Motif

    );
}
