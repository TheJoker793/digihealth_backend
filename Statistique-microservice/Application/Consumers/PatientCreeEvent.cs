namespace Statistique_microservice.Application.Consumers
{
    public record PatientCreeEvent(
    Guid PatientId, Guid CabinetId, DateTimeOffset Date);
}
