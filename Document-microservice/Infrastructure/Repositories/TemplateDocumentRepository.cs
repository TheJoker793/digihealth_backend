using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Infrastructure.Repositories
{
    public class TemplateDocumentRepository
        : Repository<TemplateDocument>,
          ITemplateDocumentRepository
    {
        public TemplateDocumentRepository(DocumentDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<TemplateDocument>> GetByTypeAsync(
            TypeDocument type,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(t => t.TypeDocument == type)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<TemplateDocument>> GetByCabinetAsync(
            Guid? cabinetId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(t => t.CabinetId == cabinetId)
                .ToListAsync(ct);
        }

        public async Task<TemplateDocument?> GetParDefautAsync(
            TypeDocument type,
            string? specialite = null,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(
                    t => t.TypeDocument == type
                      && (specialite == null || t.Specialite == specialite),
                    ct);
        }
    }
}