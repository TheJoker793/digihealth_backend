using Facturation_microservice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T>where T : class

    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();

        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            return entity != null;
        }

    }
}
