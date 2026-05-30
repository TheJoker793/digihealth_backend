using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Document_microservice.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly DocumentDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DocumentDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // ─────────────────────────────────────────────
        // Lecture
        // ─────────────────────────────────────────────

        public async Task<TEntity?> GetByIdAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            return predicate == null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);
        }

        // ─────────────────────────────────────────────
        // Écriture
        // ─────────────────────────────────────────────

        public async Task AddAsync(
            TEntity entity,
            CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(entities, ct);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}