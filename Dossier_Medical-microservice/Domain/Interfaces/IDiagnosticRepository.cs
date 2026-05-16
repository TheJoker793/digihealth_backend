using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IDiagnosticRepository : IRepository<Diagnostic>
    {
        Task<IEnumerable<Diagnostic>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default);
        Task<IEnumerable<Diagnostic>> GetByConsultationAndTypeAsync(Guid consultationId, TypeDiagnostic type, CancellationToken ct = default);
    }
}
