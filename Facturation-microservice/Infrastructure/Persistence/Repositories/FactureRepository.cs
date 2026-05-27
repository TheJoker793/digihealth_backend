using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Enums;
using Facturation_microservice.Domain.Interfaces;
using Facturation_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence.Repositories
{
    public class FactureRepository
        : Repository<Facture>, IFactureRepository
    {
        public FactureRepository(FacturationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Facture>> GetByPatientAsync(Guid patientId)
        {
            return await _dbSet
                .Where(x => x.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Facture>> GetByPeriodeAsync(
            DateOnly debut,
            DateOnly fin)
        {
            return await _dbSet
                .Where(x => x.DateFacture >= debut &&
                            x.DateFacture <= fin)
                .ToListAsync();
        }

        public async Task<IEnumerable<Facture>> GetEnRetardAsync()
        {
            return await _dbSet
                .Where(x =>
                    x.Statut == StatutFacture.EnRetard)
                .ToListAsync();
        }

        public async Task<object> GetStatsAsync()
        {
            return new
            {
                Total = await _dbSet.CountAsync()
            };
        }
    }
}