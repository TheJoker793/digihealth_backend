using Hangfire;
using Notification_microservice.Application.Services;

namespace Notification_microservice.Infrastructure.Jobs
{
    public class RetryNotificationJob
    {
        private readonly NotificationService _service;
        private readonly ILogger<RetryNotificationJob> _logger;

        public RetryNotificationJob(
        NotificationService service,
        ILogger<RetryNotificationJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 0)]  // Hangfire ne réessaie pas ce job lui-même
        public async Task ExecuterAsync()
        {
            _logger.LogInformation("RetryNotificationJob démarré.");
            await _service.ReessayerEchoueesAsync();
            _logger.LogInformation("RetryNotificationJob terminé.");
        }
    }
}
