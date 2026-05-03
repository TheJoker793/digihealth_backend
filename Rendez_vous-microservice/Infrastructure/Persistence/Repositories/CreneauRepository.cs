using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Repositories
{
    public class CreneauRepository : Repository<Creneau>, ICreneauRepository
    {
        private readonly RendezVousDbContext _context;

        public CreneauRepository(RendezVousDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Creneau>> GetDisponiblesAsync(Guid medecinId, DateTime debut, DateTime fin)
        {
            return await _context.Creneaux
                .Where(c => c.MedecinId == medecinId
                            && c.EstDisponible
                            && c.Debut >= debut
                            && c.Fin <= fin)
                .OrderBy(c => c.Debut)
                .ToListAsync();
        }

        public async Task<bool> ExisteChevaucheAsync(Guid medecinId, DateTime debut, DateTime fin)
        {
            return await _context.Creneaux
                .AnyAsync(c => c.MedecinId == medecinId
                               && c.Debut < fin
                               && debut < c.Fin);
        }
    }
}
