using Auth_microservice.Domain.Entities;
using Auth_microservice.Persistance;
using Microsoft.EntityFrameworkCore;
namespace Auth_microservice.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        // =========================
        // GET BY LOGIN
        // =========================
        public async Task<User?> GetByLoginAsync(string login, CancellationToken ct = default)
        {
            var normalized = login.ToLowerInvariant();

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Login == normalized, ct);
        }

        // =========================
        // LOGIN EXISTS
        // =========================
        public async Task<bool> LoginExistsAsync(string login, CancellationToken ct = default)
        {
            var normalized = login.ToLowerInvariant();

            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Login == normalized, ct);
        }

        // =========================
        // ADD
        // =========================
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
        }

        // =========================
        // UPDATE
        // =========================
        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}