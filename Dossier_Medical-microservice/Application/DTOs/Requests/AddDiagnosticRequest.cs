using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record AddDiagnosticRequest
    (
        string CodeCIM11,
        string LibelleCIM11,
        TypeDiagnostic TypeDiagnostic,
        string? Commentaire = null
    );
}
