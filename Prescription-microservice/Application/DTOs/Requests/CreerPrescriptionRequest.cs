using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Application.DTOs.Requests
{
    public record CreerPrescriptionRequest(
    Guid PatientId,
    Guid MedecinId,
    Guid CabinetId,
    Guid? ConsultationId,
    Guid? DossierId,
    TypePrescription TypePrescription,
    int ValiditeJours,
    string? Instructions
);

}
