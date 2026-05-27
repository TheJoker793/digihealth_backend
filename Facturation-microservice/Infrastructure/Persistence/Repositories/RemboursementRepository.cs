using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Enums;
using Facturation_microservice.Domain.Interfaces;
using Facturation_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence.Repositories
{
    public class RemboursementRepository
        : Repository<RemboursementCaisse>,
          IRemboursementRepository
    {
        public RemboursementRepository(FacturationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<RemboursementCaisse>>
            GetByFactureAsync(Guid factureId)
        {
            return await _dbSet
                .Where(x => x.FactureId == factureId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RemboursementCaisse>>
            GetEnAttenteAsync()
        {
            return await _dbSet
                .Where(x => x.Statut == StatutRemboursement.Soumis)
                .ToListAsync();
        }
    }
}