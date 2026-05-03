using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Persistance;
using Microsoft.EntityFrameworkCore;
namespace Auth_microservice.Repositories
{
    public class RefreshTokenRepository:IRefreshTokenRepository
    {
        private readonly AuthDbContext _context;

        public RefreshTokenRepository(AuthDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET BY TOKEN
        // =========================
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        // =========================
        // GET BY USER ID
        // =========================
        public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        // =========================
        // ADD
        // =========================
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        // =========================
        // UPDATE
        // =========================
        public Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return Task.CompletedTask;
        }

        // =========================
        // REVOKE SINGLE TOKEN
        // =========================
        public Task RevokeAsync(RefreshToken refreshToken)
        {
            refreshToken.Revoke();
            _context.RefreshTokens.Update(refreshToken);

            return Task.CompletedTask;
        }

        // =========================
        // REVOKE ALL USER TOKENS
        // =========================
        public async Task RevokeAllAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revoke();
            }

            _context.RefreshTokens.UpdateRange(tokens);
        }
    }
}
