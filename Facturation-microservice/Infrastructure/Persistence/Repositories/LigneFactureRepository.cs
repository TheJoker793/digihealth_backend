using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence.Repositories
{
    public class LigneFactureRepository : Repository<LigneFacture>, ILigneFactureRepository
    {
        public LigneFactureRepository(FacturationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LigneFacture>> GetByFactureAsync(Guid factureId)
        {
            return await _dbSet
                .Where(x => x.FactureId == factureId)
                .ToListAsync();
        }
    }
}
