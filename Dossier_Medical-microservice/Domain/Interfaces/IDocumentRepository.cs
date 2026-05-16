using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IDocumentRepository: IRepository<DocumentMedical>
    {
        Task<IEnumerable<DocumentMedical>> GetByDossierAsync(Guid dossierId, TypeDocument? type = null, CancellationToken ct = default);
    }
}
