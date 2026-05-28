using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Responses
{
    public record VersionDocumentResponse(
    Guid Id,
    int NumeroVersion,
    string NomFichier,
    FormatFichier Format,
    long TailleOctets,
    bool EstActive,
    string Auteur,
    int? NbPages,
    DateTimeOffset CreatedAt
);
}
