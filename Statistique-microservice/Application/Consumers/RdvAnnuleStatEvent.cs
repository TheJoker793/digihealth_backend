namespace Statistique_microservice.Application.Consumers
{
    public record RdvAnnuleStatEvent(
    Guid RdvId, Guid CabinetId);
}
