using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class PieceIdentiteRepository : Repository<PieceIdentite>, IPieceIdentiteRepository
    {
        private readonly PatientDbContext _context;
        public PieceIdentiteRepository(PatientDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PieceIdentite>> GetByPatient(Guid patientId)
        {
            return await _context.Set<PieceIdentite>()
                                 .Where(pi => pi.PatientId == patientId)
                                 .ToListAsync();
        }
    }
}
