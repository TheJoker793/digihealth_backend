using Auth_microservice.Domain.Entities;

namespace Auth_microservice.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        // =========================
        // QUERY
        // =========================
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<List<RefreshToken>> GetByUserIdAsync(Guid userId);

        // =========================
        // COMMAND
        // =========================
        Task AddAsync(RefreshToken refreshToken);

        Task UpdateAsync(RefreshToken refreshToken);

        Task RevokeAsync(RefreshToken refreshToken);

        Task RevokeAllAsync(Guid userId);
    }
}
