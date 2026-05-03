using Auth_microservice.Domain.Enums;
using System.Data;

namespace Auth_microservice.Domain.Entities
{
    public class User : BaseEntity
    {
        // =========================
        // IDENTITÉ
        // =========================
        public string Login { get; private set; } = default!;
        public string HashedPassword { get; private set; } = default!;
        public Role Role { get; private set; }
        public string CabinetId { get; private set; } = default!;
        public string? Specialite { get; private set; }

        // =========================
        // SÉCURITÉ / COMPTE
        // =========================
        public bool IsActive { get; private set; } = true;
        public int FailedAttempts { get; private set; }
        public DateTime? LockedUntil { get; private set; }

        // =========================
        // 2FA
        // =========================
        public bool Is2FAEnabled { get; private set; }
        public string? TotpSecret { get; private set; }

        // =========================
        // REFRESH TOKENS
        // =========================

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        // =========================
        // FACTORY (CREATION CONTROLLEE)
        // =========================
        public static User Create(
            string login,
        string hashedPwd,
            Role role,
            string cabinetId,
            string? specialite = null)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login.ToLowerInvariant(),
                HashedPassword = hashedPwd,
                Role = role,
                CabinetId = cabinetId,
                Specialite = specialite,
                CreatedAt = DateTimeOffset.UtcNow
            };

            user.AddDomainEvent(new UserCreatedEvent(user.Id, role));

            return user;
        }

        // =========================
        // LOGIN / SECURITY LOGIC
        // =========================
        public void RecordFailedLogin()
        {
            FailedAttempts++;

            if (FailedAttempts >= 5)
            {
                LockedUntil = DateTime.UtcNow.AddMinutes(15);
            }
        }

        public bool IsLocked()
            => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;

        public void ResetFailedAttempts()
            => FailedAttempts = 0;

        public void Activate()
            => IsActive = true;

        public void Deactivate()
            => IsActive = false;

        // =========================
        // 2FA MANAGEMENT
        // =========================
        public void Enable2FA(string totpSecret)
        {
            TotpSecret = totpSecret;
            Is2FAEnabled = true;
        }

        public void Disable2FA()
        {
            TotpSecret = null;
            Is2FAEnabled = false;
        }

        // =========================
        // REFRESH TOKENS
        // =========================
        public RefreshToken AddRefreshToken(string token, string deviceInfo)
        {
            var rt = RefreshToken.Create(token, Id, deviceInfo);
            _refreshTokens.Add(rt);
            return rt;
        }

        public void RevokeAllTokens()
        {
            foreach (var token in _refreshTokens)
            {
                token.Revoke();
            }
        }

        internal void ChangePassword(string v)
        {
            throw new NotImplementedException();
        }


        // Dans User.cs — section LOGIN / SECURITY LOGIC
        public void ChangeRole(Role newRole)
        {
            Role = newRole;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
