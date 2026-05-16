using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Enums;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Repositories
{
    public class DiagnosticRepository : Repository<Diagnostic>, IDiagnosticRepository
    {
        public DiagnosticRepository(DossierDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Diagnostic>> GetByConsultationAndTypeAsync(Guid consultationId, TypeDiagnostic type, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Diagnostic>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default)
            => await _dbSet.Where(d => d.ConsultationId == consultationId)
                           .ToListAsync(ct);
    }
}
