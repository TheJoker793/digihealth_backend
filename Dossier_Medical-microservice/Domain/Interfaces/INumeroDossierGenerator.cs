namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface INumeroDossierGenerator
    {
        Task<string> GenerateAsync(Guid cabinetId, CancellationToken ct = default);
    }
}
