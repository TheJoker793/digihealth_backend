namespace Notification_microservice.Application.Consumers
{
    public record OrdonnanceSigneeEvent(
    Guid PrescriptionId, Guid PatientId, Guid MedecinId,
    string PatientEmail, string CheminPdfMinIO);
}
