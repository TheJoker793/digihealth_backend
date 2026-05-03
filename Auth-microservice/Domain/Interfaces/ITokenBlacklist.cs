namespace Auth_microservice.Domain.Interfaces
{
    public interface ITokenBlacklist
    {
        Task BlacklistAsync(string jti, TimeSpan ttl);

        Task<bool> IsBlacklistedAsync(string jti);
    }
}
