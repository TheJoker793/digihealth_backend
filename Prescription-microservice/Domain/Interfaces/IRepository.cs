
using Prescription_microservice.Domain.Entities;
using System.Linq.Expressions;

namespace Prescription_microservice.Domain.Interfaces
{


    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);
        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default);
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        Task UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task DeleteAsync(TEntity entity, CancellationToken ct = default);
    }


}
