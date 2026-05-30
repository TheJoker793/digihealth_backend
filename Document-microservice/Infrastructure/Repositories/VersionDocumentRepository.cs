using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Infrastructure.Repositories
{
    public class VersionDocumentRepository
        : Repository<VersionDocument>,
          IVersionDocumentRepository
    {
        public VersionDocumentRepository(DocumentDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<VersionDocument>> GetByDocumentAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(v => v.DocumentMedicalId == documentId)
                .OrderByDescending(v => v.NumeroVersion)
                .ToListAsync(ct);
        }

        public async Task<VersionDocument?> GetVersionActiveAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(
                    v => v.DocumentMedicalId == documentId && v.EstActive,
                    ct);
        }

        public async Task<VersionDocument?> GetByNumeroAsync(
            Guid documentId,
            int numeroVersion,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(
                    v => v.DocumentMedicalId == documentId &&
                         v.NumeroVersion == numeroVersion,
                    ct);
        }
    }
}