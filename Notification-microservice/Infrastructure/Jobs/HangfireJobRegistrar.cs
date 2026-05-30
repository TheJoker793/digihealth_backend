using Hangfire;

namespace Notification_microservice.Infrastructure.Jobs
{
    public static class HangfireJobRegistrar
    {
        public static void EnregistrerJobs()
        {
            // Toutes les minutes — envoyer les notifications programmées
            RecurringJob.AddOrUpdate<EnvoyerProgrammeesJob>(
                "envoyer-programmees",
                job => job.ExecuterAsync(),
                "* * * * *");

            // Toutes les 15 minutes — retry des échouées
            RecurringJob.AddOrUpdate<RetryNotificationJob>(
                "retry-echouees",
                job => job.ExecuterAsync(),
                "*/15 * * * *");

            // Quotidien à 02:00 — nettoyage historiques
            RecurringJob.AddOrUpdate<CleanupHistoriqueJob>(
                "cleanup-historique",
                job => job.ExecuterAsync(),
                "0 2 * * *");
        }
    }
}
