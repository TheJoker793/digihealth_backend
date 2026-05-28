using Document_microservice.Domain.Entities;

namespace Document_microservice.Domain.Interfaces
{
    public interface IPartageDocumentRepository : IRepository<PartageDocument>
    {
        Task<PartageDocument?> GetByTokenAsync(
        string token,
        CancellationToken ct = default);

        Task<IEnumerable<PartageDocument>> GetByDocumentAsync(
            Guid documentId,
            CancellationToken ct = default);

        Task<IEnumerable<PartageDocument>> GetByDestinataireAsync(
            Guid destinataireId,
            CancellationToken ct = default);
    }
}
