using Hangfire;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public class ConsolidationSnapshotJob
    {
        private readonly SnapshotService _snapshotService;
        private readonly ILogger<ConsolidationSnapshotJob> _logger;

        public ConsolidationSnapshotJob(
            SnapshotService snapshotService,
            ILogger<ConsolidationSnapshotJob> logger)
        {
            _snapshotService = snapshotService;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 1)]
        public async Task ExecuterAsync()
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            _logger.LogInformation("ConsolidationSnapshotJob — date {Date}", date);
            await _snapshotService.ConsoliderTousCabinetsAsync(date);
        }
    }
}
