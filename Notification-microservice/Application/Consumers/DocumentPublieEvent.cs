namespace Notification_microservice.Application.Consumers
{
    public record DocumentPublieEvent(
    Guid DocumentId, Guid PatientId,
    string TypeDocument, string TitreDocument,
    string PatientEmail, string? TokenFcm);
}
