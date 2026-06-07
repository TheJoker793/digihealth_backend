namespace Statistique_microservice.Application.Consumers
{
    public record PrescriptionSigneeStatEvent(
    Guid PrescriptionId, Guid CabinetId, DateTimeOffset Date);
}
