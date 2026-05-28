using Document_microservice.Domain.Enums;

namespace Document_microservice.Application.DTOs.Responses
{
    public record DocumentResponse(
    Guid Id,
    string Numero,
    string Titre,
    string? Description,
    TypeDocument TypeDocument,
    StatutDocument Statut,
    bool EstArchive,
    Guid PatientId,
    Guid MedecinId,
    Guid CabinetId,
    Guid? ConsultationId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    VersionDocumentResponse? VersionActive
);
}
