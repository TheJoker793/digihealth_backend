using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class AssuranceComplementaireRepository : Repository<AssuranceComplementaire>, IAssuranceComplementaireRepository
    {
        private readonly PatientDbContext _context;
        public AssuranceComplementaireRepository(PatientDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AssuranceComplementaire>> GetActiveByPatient(Guid patientId)
        {
            return await _context.AssuranceComplementaires
                                 .Where(ac => ac.PatientId == patientId && ac.DateFin >= DateOnly.FromDateTime(DateTime.UtcNow))
                                 .ToListAsync();
        }
    }
}
