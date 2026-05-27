namespace Facturation_microservice.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> FindAsync();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> CountAsync();

        Task<bool> ExistsAsync(Guid id);
    }
}