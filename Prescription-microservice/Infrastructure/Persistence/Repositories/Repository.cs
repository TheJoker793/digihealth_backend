using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Prescription_microservice.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly PrescriptionDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(PrescriptionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate == null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);

        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await _dbSet.AddRangeAsync(entities, ct);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
