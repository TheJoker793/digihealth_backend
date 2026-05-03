using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Repositories
{
    public class RendezVousRepository : Repository<RendezVous>, IRendezVousRepository
    {
        private readonly RendezVousDbContext _context;

        public RendezVousRepository(RendezVousDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RendezVous>> GetByPatientAsync(Guid patientId)
        {
            return await _context.RendezVous
                .Where(r => r.PatientId == patientId)
                .OrderBy(r => r.DateHeure)
                .ToListAsync();
        }

        public async Task<IEnumerable<RendezVous>> GetByMedecinAsync(Guid medecinId)
        {
            return await _context.RendezVous
                .Where(r => r.MedecinId == medecinId)
                .OrderBy(r => r.DateHeure)
                .ToListAsync();
        }

        public async Task<IEnumerable<RendezVous>> GetAgendaAsync(Guid medecinId, DateTime debut, DateTime fin)
        {
            return await _context.RendezVous
                .Where(r => r.MedecinId == medecinId && r.DateHeure >= debut && r.DateHeure <= fin)
                .OrderBy(r => r.DateHeure)
                .ToListAsync();
        }
    }
}
