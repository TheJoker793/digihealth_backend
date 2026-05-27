using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Interfaces;
using Facturation_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence.Repositories
{
    public class PaiementRepository
        : Repository<Paiement>, IPaiementRepository
    {
        public PaiementRepository(FacturationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Paiement>> GetByFactureAsync(Guid factureId)
        {
            return await _dbSet
                .Where(x => x.FactureId == factureId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paiement>> GetByPeriodeAsync(
            DateOnly debut,
            DateOnly fin)
        {
            return await _dbSet
                .Where(x =>
                    DateOnly.FromDateTime(x.Date.DateTime) >= debut &&
                    DateOnly.FromDateTime(x.Date.DateTime) <= fin)
                .ToListAsync();
        }
    }
}