namespace Auth_microservice.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeAsync(string email);

        Task SendResetPasswordAsync(string email, string token);

        Task SendAlertAsync(string email, string message);
    }
}
