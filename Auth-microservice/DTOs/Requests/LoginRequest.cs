namespace Auth_microservice.DTOs.Requests
{
    public record LoginRequest(
        string Login,
        string Password
    );
}
