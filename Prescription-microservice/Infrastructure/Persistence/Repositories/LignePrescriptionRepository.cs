using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Persistence;

namespace Prescription_microservice.Infrastructure.Persistence.Repositories
{
    public class LignePrescriptionRepository : Repository<LignePrescription>, ILignePrescriptionRepository
    {
        public LignePrescriptionRepository(PrescriptionDbContext context) : base(context) { }

        public async Task<IEnumerable<LignePrescription>> GetByPrescriptionAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(l => l.PrescriptionId == prescriptionId)
                .Include(l => l.Posologie) // inclut la valeur-objet Posologie
                .ToListAsync(ct);
        }
    }
}
