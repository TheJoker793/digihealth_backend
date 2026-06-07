namespace Auth_microservice.DTOs.Requests
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword
    );
}
