using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Events;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Application.Services
{
    public class RappelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;

        public RappelService(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }
        public async Task ProgrammerRappels(RendezVous rdv)
        {
            // Exemple : programmer rappel J-1 et H-2 via Hangfire
            var rappelJ1 = rdv.DateHeure.AddDays(-1);
            var rappelH2 = rdv.DateHeure.AddHours(-2);

            // Ici tu pourrais planifier des jobs Hangfire
            // BackgroundJob.Schedule(() => EnvoyerRappelJ1(rdv.Id), rappelJ1);
            // BackgroundJob.Schedule(() => EnvoyerRappelH2(rdv.Id), rappelH2);
        }

        public async Task EnvoyerRappelJ1(Guid rendezVousId)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            await _eventPublisher.PublishAsync(new RappelRdvEvent(rdv.Id, rdv.PatientId, DateTime.UtcNow));
        }
        public async Task EnvoyerRappelH2(Guid rendezVousId)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            await _eventPublisher.PublishAsync(new RappelRdvEvent(rdv.Id, rdv.PatientId, DateTime.UtcNow));
        }
    }
}
