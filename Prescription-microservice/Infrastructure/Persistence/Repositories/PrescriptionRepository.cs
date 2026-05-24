using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Persistence;

namespace Prescription_microservice.Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(PrescriptionDbContext context) : base(context) { }

        public async Task<IEnumerable<Prescription>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Prescription>> GetActivesAsync(Guid patientId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.PatientId == patientId && p.Statut == Domain.Enums.StatutPrescription.Validee && !p.IsExpired())
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Prescription>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.ConsultationId == consultationId)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Prescription>> GetExpireesAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .Where(p => p.Statut == Domain.Enums.StatutPrescription.Validee && p.IsExpired())
                .ToListAsync(ct);
        }

        public async Task<Prescription?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(p => p.Lignes)
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<Prescription?> GetByNumeroAsync(string numero, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.NumeroPrescription.Valeur == numero, ct);
        }
    }
}
