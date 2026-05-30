using Hangfire;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Jobs
{
    public class CleanupHistoriqueJob
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CleanupHistoriqueJob> _logger;
        private const int RetentionJours = 90;

        public CleanupHistoriqueJob(IUnitOfWork uow, ILogger<CleanupHistoriqueJob> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 1)]
        public async Task ExecuterAsync()
        {
            var limite = DateTimeOffset.UtcNow.AddDays(-RetentionJours);

            var anciens = await _uow.Notifications.FindAsync(
                n => n.Statut == Domain.Enums.StatutNotification.Envoye
                  && n.DateEnvoi < limite);

            foreach (var n in anciens)
                _uow.Notifications.Remove(n);

            var nb = anciens.Count();
            await _uow.SaveChangesAsync();

            _logger.LogInformation(
                "CleanupHistoriqueJob : {Nb} notifications > {J}j supprimées.", nb, RetentionJours);
        }
    }
}
