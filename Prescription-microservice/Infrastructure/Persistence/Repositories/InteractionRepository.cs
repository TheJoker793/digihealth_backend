using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Persistence;

namespace Prescription_microservice.Infrastructure.Persistence.Repositories
{
    public class InteractionRepository : Repository<InteractionDetectee>, IInteractionRepository
    {
        public InteractionRepository(PrescriptionDbContext context) : base(context) { }

        public async Task<IEnumerable<InteractionDetectee>> GetByPrescriptionAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(i => i.PrescriptionId == prescriptionId)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<InteractionDetectee>> GetNonContourneesAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(i => i.PrescriptionId == prescriptionId && !i.EstContournee)
                .ToListAsync(ct);
        }
    }
}





