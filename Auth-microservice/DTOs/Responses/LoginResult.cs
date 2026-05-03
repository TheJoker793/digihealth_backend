namespace Auth_microservice.DTOs.Responses
{
    public record LoginResult(
    bool Requires2FA,
    string? AccessToken,
    Guid? UserId
);
}
