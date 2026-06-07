using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface INumeroRapportGenerator
    {
        Task<string> GenererAsync(Guid cabinetId, TypeRapport type, CancellationToken ct = default);
    }
}
