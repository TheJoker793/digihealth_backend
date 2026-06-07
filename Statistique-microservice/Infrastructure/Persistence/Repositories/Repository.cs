using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Interfaces;
using System.Linq.Expressions;

namespace Statistique_microservice.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
            where TEntity : BaseEntity

    {
        protected readonly StatistiqueDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(StatistiqueDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);

        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity, ct);

        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities, CancellationToken ct = default)
            => await _dbSet.AddRangeAsync(entities, ct);

        public void Update(TEntity entity) => _dbSet.Update(entity);
        public void Remove(TEntity entity) => _dbSet.Remove(entity);
    }
}
