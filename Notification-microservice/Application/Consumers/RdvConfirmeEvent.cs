namespace Notification_microservice.Application.Consumers
{
    public record RdvConfirmeEvent(
    Guid RdvId, Guid PatientId, Guid MedecinId,
    DateTimeOffset DateRdv, string MotifRdv,
    string PatientEmail, string PatientTelephone,
    string MedecinNom, string CabinetNom);
}
