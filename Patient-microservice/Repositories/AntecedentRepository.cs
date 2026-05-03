using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class AntecedentRepository : Repository<Antecedent>, IAntecedentRepository
    {
        private readonly PatientDbContext _context;

        public AntecedentRepository(PatientDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Antecedent>> GetAntecedentsByPatientAsync(Guid patientId)
        {
            return await _context.Antecedents
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }
    }
}
