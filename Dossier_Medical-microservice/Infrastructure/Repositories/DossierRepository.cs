using Dossier_Medical_microservice.Domain.Entities;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Repositories
{
    public class DossierRepository : Repository<DossierMedical>, IDossierRepository
    {
        public DossierRepository(DossierDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DossierMedical>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
            => await _dbSet.Where(d => d.PatientId == patientId)
                           .OrderByDescending(d => d.DateOuverture)
                           .ToListAsync(ct);

        public async Task<DossierMedical?> GetByNumeroDossierAsync(string numero, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(d => d.NumeroDossier.Valeur == numero, ct);

        public async Task<bool> ExistsForPatientAsync(Guid patientId, Guid medecinId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(d => d.PatientId == patientId && d.MedecinId == medecinId, ct);
    }
}
