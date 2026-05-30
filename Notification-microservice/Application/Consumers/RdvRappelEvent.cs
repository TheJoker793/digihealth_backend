namespace Notification_microservice.Application.Consumers
{
    public record RdvRappelEvent(
    Guid RdvId, Guid PatientId,
    DateTimeOffset DateRdv, bool EstRappel24h,
    string PatientEmail, string PatientTelephone, string? TokenFcm);
}
