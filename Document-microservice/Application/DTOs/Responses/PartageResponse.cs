namespace Document_microservice.Application.DTOs.Responses
{
    public record PartageResponse(
    Guid Id,
    Guid DocumentId,
    string TokenAcces,
    Guid DestinataireId,
    string TypeDestinataire,
    bool LectureSeule,
    int NombreAcces,
    int? LimiteAcces,
    DateTime? DateExpiration,
    bool EstValide,
    DateTimeOffset CreatedAt
);

}
