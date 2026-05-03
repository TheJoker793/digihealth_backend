using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class CouvertureSocialeRepository : Repository<CouvertureSociale>, ICouvertureSocialeRepository
    {
        private readonly PatientDbContext _context;
        public CouvertureSocialeRepository(PatientDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CouvertureSociale>> GetActiveByPatient(Guid patientId)
        {
            return await _context.CouvertureSociales
                                 .Where(cs => cs.PatientId == patientId && cs.DateFin >= DateOnly.FromDateTime(DateTime.UtcNow))
                                 .ToListAsync();
        }
    }
}
