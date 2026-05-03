using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.DTOs.Responses;
using Auth_microservice.Exceptions;

namespace Auth_microservice.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwt;
        private readonly IPasswordHasher _hasher;
        private readonly ITotpService _totp;

        public AuthService(
            IUnitOfWork uow,
            IJwtService jwt,
            IPasswordHasher hasher,
            ITotpService totp)
        {
            _uow = uow;
            _jwt = jwt;
            _hasher = hasher;
            _totp = totp;
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<AuthResponse> LoginAsync(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                throw new UnauthorizedException("Invalid credentials");

            var user = await _uow.Users.GetByLoginAsync(login.ToLowerInvariant())
                ?? throw new NotFoundException("User not found");

            if (!user.IsActive)
                throw new ConflictException("Account disabled");

            if (user.IsLocked())
                throw new AccountLockedException("Account locked");

            if (!_hasher.Verify(password, user.HashedPassword))
            {
                user.RecordFailedLogin();
                await _uow.SaveChangesAsync();
                throw new UnauthorizedException("Invalid credentials");
            }

            user.ResetFailedAttempts();
            await _uow.SaveChangesAsync();

            if (user.Is2FAEnabled)
                throw new UnauthorizedException("2FA_REQUIRED");

            return await GenerateTokensAsync(user);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<AuthResponse> RefreshAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedException("Invalid refresh token");

            var token = await _uow.RefreshTokens.GetByTokenAsync(refreshToken)
                ?? throw new UnauthorizedException("Invalid refresh token");

            if (!token.IsActive())
                throw new UnauthorizedException("Expired refresh token");

            var user = await _uow.Users.GetByIdAsync(token.UserId)
                ?? throw new NotFoundException("User not found");

            // 🔥 rotation
            token.Revoke();

            var newRefreshToken = GenerateRefreshToken();

            await _uow.RefreshTokens.AddAsync(RefreshToken.Create(
                newRefreshToken,
                user.Id,
                token.DeviceInfo
            ));

            await _uow.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = _jwt.GenerateAccessToken(user),
                RefreshToken = newRefreshToken
            };
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _uow.RefreshTokens.GetByTokenAsync(refreshToken);

            if (token == null)
                return;

            token.Revoke();

            await _uow.SaveChangesAsync();
        }

        // =========================
        // INTERNAL: TOKEN GENERATION
        // =========================
        private async Task<AuthResponse> GenerateTokensAsync(User user)
        {
            var accessToken = _jwt.GenerateAccessToken(user);

            var refreshToken = GenerateRefreshToken();

            await _uow.RefreshTokens.AddAsync(
                RefreshToken.Create(refreshToken, user.Id, "web")
            );

            await _uow.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<string> Verify2FAAsync(Guid userId, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new UnauthorizedException("Invalid 2FA code");

            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            if (!user.Is2FAEnabled || string.IsNullOrEmpty(user.TotpSecret))
                throw new ConflictException("2FA not enabled");

            var isValid = _totp.ValidateCode(user.TotpSecret, code);

            if (!isValid)
                throw new UnauthorizedException("Invalid 2FA code");

            // ✅ Après validation 2FA → on génère tokens complets
            return _jwt.GenerateAccessToken(user);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.RandomNumberGenerator.GetBytes(64)
            );
        }
    }
}