using Document_microservice.Domain.Entities;

namespace Document_microservice.Domain.Interfaces
{
    public interface IVersionDocumentRepository : IRepository<VersionDocument>
    {
        Task<IEnumerable<VersionDocument>> GetByDocumentAsync(
        Guid documentId,
        CancellationToken ct = default);

        Task<VersionDocument?> GetVersionActiveAsync(
            Guid documentId,
            CancellationToken ct = default);

        Task<VersionDocument?> GetByNumeroAsync(
            Guid documentId,
            int numeroVersion,
            CancellationToken ct = default);
    }
}
