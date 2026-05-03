using Auth_microservice.Domain.Entities;
using System.Security.Claims;

namespace Auth_microservice.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        ClaimsPrincipal? ValidateToken(string token);

        string GetJtiFromToken(string token);
    }
}
