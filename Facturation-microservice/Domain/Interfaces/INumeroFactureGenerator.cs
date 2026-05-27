namespace Facturation_microservice.Domain.Interfaces
{
    public interface INumeroFactureGenerator
    {
        Task<string> GenerateAsync(Guid cabinetId);

    }
}
