using Auth_microservice.Domain.Interfaces;

namespace Auth_microservice.Services
{
    public class SendGridEmailService : IEmailService
    {
        public Task SendWelcomeAsync(string email)
        {
            return Task.CompletedTask;
        }

        public Task SendResetPasswordAsync(string email, string link)
        {
            return Task.CompletedTask;
        }

        public Task SendAlertAsync(string email, string message)
        {
            return Task.CompletedTask;
        }

        
    }
}
