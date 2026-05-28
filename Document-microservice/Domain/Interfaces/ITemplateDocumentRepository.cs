using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;

namespace Document_microservice.Domain.Interfaces
{
    public interface ITemplateDocumentRepository : IRepository<TemplateDocument>
    {
        Task<IEnumerable<TemplateDocument>> GetByTypeAsync(
        TypeDocument type,
        CancellationToken ct = default);

        Task<IEnumerable<TemplateDocument>> GetByCabinetAsync(
            Guid? cabinetId,
            CancellationToken ct = default);

        Task<TemplateDocument?> GetParDefautAsync(
            TypeDocument type,
            string? specialite = null,
            CancellationToken ct = default);
    }
}
