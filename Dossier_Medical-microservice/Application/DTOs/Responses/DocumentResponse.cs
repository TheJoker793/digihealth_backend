using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record DocumentResponse(
        Guid Id,
        TypeDocument TypeDocument,
        string Titre,
        DateOnly DateDocument,
        string MimeType,
        long TailleOctets,
        string? DownloadUrl
    );
}
