using Hangfire;
using Rendez_vous_microservice.Application.Services;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Services
{
    public class HangfireRappelJob
    {
        private readonly IRappelService _rappelService;

        public HangfireRappelJob(IRappelService rappelService)
        {
            _rappelService = rappelService;
        }

        // Job récurrent toutes les heures
        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuterAsync()
        {
            await _rappelService.EnvoyerRappelJ1();
            await _rappelService.EnvoyerRappelH2();
        }
    }
}
