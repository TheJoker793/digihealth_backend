using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Repositories
{
    public class DocumentRepository : Repository<DocumentMedical>, IDocumentRepository
    {
        public DocumentRepository(DossierDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DocumentMedical>> GetByDossierAsync(Guid dossierId, TypeDocument? type = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(d => d.DossierId == dossierId);
            if (type.HasValue)
                query = query.Where(d => d.TypeDocument == type.Value);

            return await query.ToListAsync(ct);
        }
    }
}
