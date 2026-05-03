namespace Auth_microservice.DTOs.Responses
{
    public record TokenPair(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
}
