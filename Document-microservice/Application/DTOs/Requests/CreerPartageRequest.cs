namespace Document_microservice.Application.DTOs.Requests
{
    public record CreerPartageRequest(
    Guid DocumentId,
    Guid DestinataireId,
    string TypeDestinataire,       // "Medecin" | "Patient" | "Externe"
    bool LectureSeule = true,
    DateTime? DateExpiration = null,
    int? LimiteAcces = null
);
}
