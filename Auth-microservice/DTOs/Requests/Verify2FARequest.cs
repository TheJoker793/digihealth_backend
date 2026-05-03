namespace Auth_microservice.DTOs.Requests
{
    public record Verify2FARequest(
    Guid UserId,
    string Code
    );
    
}
