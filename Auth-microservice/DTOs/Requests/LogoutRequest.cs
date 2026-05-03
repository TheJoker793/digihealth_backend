namespace Auth_microservice.DTOs.Requests
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}