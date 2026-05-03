namespace Auth_microservice.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public string DeviceInfo { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }

        // =========================
        // FACTORY
        // =========================
        public static RefreshToken Create(string token, Guid userId, string deviceInfo)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                UserId = userId,
                DeviceInfo = deviceInfo,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        // =========================
        // DOMAIN METHODS (MANQUANTS CHEZ TOI)
        // =========================
        public bool IsExpired()
            => DateTime.UtcNow > ExpiresAt;

        public bool IsActive()
            => !IsRevoked && !IsExpired();

        public void Revoke()
        {
            IsRevoked = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}