using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Responses
{
    public record UrlPresigneeResponse(
    string Url,
    string NomFichier,
    FormatFichier Format,
    long TailleOctets,
    DateTime ExpirationUtc
);
}
