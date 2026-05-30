namespace Notification_microservice.Application.Consumers
{
    public record RdvAnnuleEvent(
    Guid RdvId, Guid PatientId, Guid MedecinId,
    DateTimeOffset DateRdv, string MotifAnnulation,
    string PatientEmail, string PatientTelephone);
}
