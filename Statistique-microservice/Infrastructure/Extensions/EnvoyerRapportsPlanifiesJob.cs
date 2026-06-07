using Hangfire;
using Statistique_microservice.Application.Services;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public class EnvoyerRapportsPlanifiesJob
    {
        private readonly AbonnementService _abonnementService;
        private readonly RapportService _rapportService;
        private readonly IUnitOfWork _uow;

        public EnvoyerRapportsPlanifiesJob(
            AbonnementService abonnementService,
            RapportService rapportService,
            IUnitOfWork uow)
        {
            _abonnementService = abonnementService;
            _rapportService = rapportService;
            _uow = uow;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuterAsync()
        {
            // Rapports planifiés (date unique)
            var planifies = await _uow.Rapports.GetPlanifiesAsync();
            foreach (var rapport in planifies)
                await _rapportService.ExecuterGenerationAsync(rapport);

            // Abonnements récurrents échus
            await _abonnementService.TraiterAbonnementsEchusAsync();
        }
    }
}
