using Hangfire;
using Notification_microservice.Application.Services;

namespace Notification_microservice.Infrastructure.Jobs
{
    public class EnvoyerProgrammeesJob
    {
        private readonly NotificationService _service;
        private readonly ILogger<EnvoyerProgrammeesJob> _logger;

        public EnvoyerProgrammeesJob(
            NotificationService service,
            ILogger<EnvoyerProgrammeesJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuterAsync()
        {
            _logger.LogDebug("EnvoyerProgrammeesJob démarré.");
            await _service.EnvoyerProgrammeesAsync();
        }
    }
}
