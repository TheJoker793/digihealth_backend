namespace Document_microservice.Application.DTOs.Requests
{
    public record GenererPdfRequest(
    Guid TemplateId,
    Guid PatientId,
    Guid MedecinId,
    Guid CabinetId,
    Dictionary<string, string> Variables,
    Guid? ConsultationId = null
);
}
