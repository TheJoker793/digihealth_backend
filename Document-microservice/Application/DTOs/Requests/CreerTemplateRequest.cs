using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Requests
{
    public record CreerTemplateRequest(
    string Nom,
    TypeDocument TypeDocument,
    string ContenuHtml,
    string[] Variables,
    string Version = "1.0",
    string? Specialite = null,
    Guid? CabinetId = null,
    string? Description = null
);
}
