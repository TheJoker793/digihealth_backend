namespace Statistique_microservice.Application.Consumers
{
    public record FactureGenereeStatEvent(
    Guid FactureId, Guid CabinetId,
    decimal Montant, bool EstImpayee, DateTimeOffset Date);
}
