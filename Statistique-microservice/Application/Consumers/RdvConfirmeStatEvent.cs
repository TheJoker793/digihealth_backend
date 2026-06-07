namespace Statistique_microservice.Application.Consumers
{
    public record RdvConfirmeStatEvent(
    Guid RdvId, Guid CabinetId, DateTimeOffset DateRdv);

}
