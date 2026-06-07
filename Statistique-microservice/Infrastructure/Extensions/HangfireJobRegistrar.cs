using Hangfire;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public static class HangfireJobRegistrar
    {
        public static void EnregistrerJobs()
        {
            // Quotidien à 23h55 — consolidation snapshots
            RecurringJob.AddOrUpdate<ConsolidationSnapshotJob>(
                "consolidation-snapshots",
                job => job.ExecuterAsync(),
                "55 23 * * *");

            // Toutes les heures — rapports planifiés + abonnements échus
            RecurringJob.AddOrUpdate<EnvoyerRapportsPlanifiesJob>(
                "envoyer-rapports-planifies",
                job => job.ExecuterAsync(),
                "0 * * * *");

            // Premier du mois à 03h00 — purge RGPD
            RecurringJob.AddOrUpdate<PurgerAnciensSnapshotsJob>(
                "purge-anciens-snapshots",
                job => job.ExecuterAsync(),
                "0 3 1 * *");
        }
    }
}
