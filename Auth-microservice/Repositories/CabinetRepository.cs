using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Auth_microservice.Repositories
{
    public class CabinetRepository : ICabinetRepository
    {
        private readonly AuthDbContext _context;

        public CabinetRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Cabinet> AddAsync(Cabinet cabinet)
        {
            await _context.Cabinets.AddAsync(cabinet);
            return cabinet;
        }

        public async Task<Cabinet?> GetByIdAsync(Guid id)
        {
            return await _context.Cabinets
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cabinet>> GetAllAsync()
        {
            return await _context.Cabinets
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(Cabinet cabinet)
        {
            _context.Cabinets.Update(cabinet);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var cabinet = await _context.Cabinets.FindAsync(id);

            if (cabinet != null)
                _context.Cabinets.Remove(cabinet);
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            return _context.Cabinets.AnyAsync(c => c.Email == email);
        }

        public Task<bool> ExistsByTelephoneAsync(string telephone)
        {
            return _context.Cabinets.AnyAsync(c => c.Telephone == telephone);
        }

        public async Task<IEnumerable<Cabinet>> GetActiveAsync()
        {
            return await _context.Cabinets
                .Where(c => c.Actif)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}