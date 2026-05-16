using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record DiagnosticResponse(
        Guid Id,
        string CodeCIM11,
        string LibelleCIM11,
        TypeDiagnostic TypeDiagnostic,
        string? Commentaire,
        bool EstConfirme,
        DateTimeOffset CreatedAt
    );
}
