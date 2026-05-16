using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Repositories
{
    public class OrdonnanceRepository : Repository<Ordonnance>, IOrdonnanceRepository
    {
        public OrdonnanceRepository(DossierDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ordonnance>> GetActiveByPatientAsync(Guid patientId, CancellationToken ct = default)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow); // 🔹 conversion explicite

            return await _dbSet
                .Include(o => o.Consultation) // pour naviguer vers Consultation
                .ThenInclude(c => c.DossierMedical)  // pour naviguer vers Dossier
                .Where(o => o.Consultation != null &&
                        o.Consultation.DossierMedical != null &&
                        o.Consultation.DossierMedical.PatientId == patientId &&
                        o.Date.AddDays(o.ValiditeJours) >= today)
                .ToListAsync(ct);
        }


        public async Task<IEnumerable<Ordonnance>> GetByConsultationAsync(Guid consultationId, CancellationToken ct = default)
            => await _dbSet.Where(o => o.ConsultationId == consultationId)
                           .ToListAsync(ct);
    }
}
