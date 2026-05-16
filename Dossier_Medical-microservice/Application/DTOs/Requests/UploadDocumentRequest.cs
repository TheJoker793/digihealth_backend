using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record UploadDocumentRequest(
        Guid DossierId,
        TypeDocument TypeDocument,
        string Titre,
        Stream Contenu,
        string FileName,
        string MimeType
    );
}
