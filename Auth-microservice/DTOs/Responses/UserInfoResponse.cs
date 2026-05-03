using Auth_microservice.Domain.Enums;

namespace Auth_microservice.DTOs.Responses
{
    public record UserInfoResponse(
    Guid Id,
    string Login,
    Role Role,
    bool IsActive,
    bool Is2FAEnabled
);
}
