using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Infrastructure.Repositories
{
    public class PartageDocumentRepository
        : Repository<PartageDocument>,
          IPartageDocumentRepository
    {
        public PartageDocumentRepository(DocumentDbContext context)
            : base(context)
        {
        }

        public async Task<PartageDocument?> GetByTokenAsync(
            string token,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.TokenAcces == token, ct);
        }

        public async Task<IEnumerable<PartageDocument>> GetByDocumentAsync(
            Guid documentId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.DocumentMedicalId == documentId)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<PartageDocument>> GetByDestinataireAsync(
            Guid destinataireId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.DestinataireId == destinataireId)
                .ToListAsync(ct);
        }
    }
}