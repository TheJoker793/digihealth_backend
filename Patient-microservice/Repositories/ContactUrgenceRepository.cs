using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class ContactUrgenceRepository : Repository<ContactUrgence>, IContactUrgenceRepository
    {
        private readonly PatientDbContext _context;
        public ContactUrgenceRepository(PatientDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ContactUrgence>> GetByPatient(Guid patientId)
        {
            return await _context.ContactsUrgences
                                 .Where(cu => cu.PatientId == patientId)
                                 .ToListAsync();
        }
    }
}
