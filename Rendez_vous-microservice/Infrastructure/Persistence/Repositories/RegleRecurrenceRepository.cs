using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Repositories
{
    public class RegleRecurrenceRepository : Repository<RegleRecurrence>, IRegleRecurrenceRepository
    {
        private readonly RendezVousDbContext _context;

        public RegleRecurrenceRepository(RendezVousDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegleRecurrence>> GetByMedecinAsync(Guid medecinId)
        {
            return await _context.ReglesRecurrence
                .Where(r => r.MedecinId == medecinId)
                .OrderBy(r => r.DateDebut)
                .ToListAsync();
        }

        // Les méthodes AddAsync, Update, Remove, GetByIdAsync, GetAllAsync
        // sont déjà héritées du Repository<T>.
    }
}
