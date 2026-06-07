using Hangfire;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public class PurgerAnciensSnapshotsJob
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<PurgerAnciensSnapshotsJob> _logger;
        private const int RetentionAns = 2;

        public PurgerAnciensSnapshotsJob(
            IUnitOfWork uow, ILogger<PurgerAnciensSnapshotsJob> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 1)]
        public async Task ExecuterAsync()
        {
            var limite = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-RetentionAns));
            var anciens = await _uow.Snapshots.FindAsync(
                s => s.DateSnapshot < limite && s.EstConsolide);

            foreach (var s in anciens)
                _uow.Snapshots.Remove(s);

            await _uow.SaveChangesAsync();
            _logger.LogInformation(
                "PurgeSnapshot : {Nb} snapshots > {Ans} ans supprimés.",
                anciens.Count(), RetentionAns);
        }
    }

}
