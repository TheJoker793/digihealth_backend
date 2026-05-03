namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);

        void Update(T entity);       // ✔️ sync
        void Remove(T entity);       // ✔️ sync
    }
}
