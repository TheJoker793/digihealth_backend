using Auth_microservice.Domain.Entities;

namespace Auth_microservice.Domain.Interfaces
{
    public interface ICabinetRepository
    {
        // =====================
        // CREATE
        // =====================
        Task<Cabinet> AddAsync(Cabinet cabinet);

        // =====================
        // READ
        // =====================
        Task<Cabinet?> GetByIdAsync(Guid id);
        Task<IEnumerable<Cabinet>> GetAllAsync();

        // =====================
        // UPDATE
        // =====================
        Task UpdateAsync(Cabinet cabinet);

        // =====================
        // DELETE
        // =====================
        Task DeleteAsync(Guid id);

        // =====================
        // BUSINESS RULES
        // =====================
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByTelephoneAsync(string telephone);

        // =====================
        // FILTERS (utile SaaS multi-cabinets)
        // =====================
        Task<IEnumerable<Cabinet>> GetActiveAsync();
    }
}
