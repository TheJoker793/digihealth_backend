using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Requests
{
    public record UploadDocumentRequest(
    string Titre,
    TypeDocument TypeDocument,
    Guid PatientId,
    Guid MedecinId,
    Guid CabinetId,
    IFormFile Fichier,
    Guid? ConsultationId = null,
    Guid? PrescriptionId = null,
    string? Description = null,
    string? MotsCles = null        // séparés par virgule
);

}
