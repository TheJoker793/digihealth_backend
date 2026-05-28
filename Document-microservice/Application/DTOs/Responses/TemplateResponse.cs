using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Responses
{
    public record TemplateResponse(
    Guid Id,
    string Nom,
    string? Description,
    TypeDocument TypeDocument,
    string? Specialite,
    string[] Variables,
    string Version,
    bool EstActif,
    Guid? CabinetId,
    DateTimeOffset CreatedAt
);
}
