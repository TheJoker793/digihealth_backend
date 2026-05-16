using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Repositories
{
    public class ConsultationRepository : Repository<Consultation>, IConsultationRepository
    {
        public ConsultationRepository(DossierDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Consultation>> GetByDossierAsync(Guid dossierId, CancellationToken ct = default)
            => await _dbSet.Where(c => c.DossierId == dossierId)
                           .ToListAsync(ct);
        public async Task<IEnumerable<Consultation>> GetHistoriqueAsync(Guid dossierId, CancellationToken ct = default)
            => await _dbSet.Where(c => c.DossierId == dossierId)
                           .OrderByDescending(c => c.Date)
                           .ToListAsync(ct);
    }
}
