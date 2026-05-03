using Microsoft.AspNetCore.SignalR;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Services
{
    public class SignalRRdvNotifier: IRdvNotifier
    {
        private readonly IHubContext<RendezVousHub> _hubContext;

        public SignalRRdvNotifier(IHubContext<RendezVousHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifierNouveauRdvAsync(Guid medecinId, object rendezVous)
        {
            await _hubContext.Clients.Group(medecinId.ToString())
                .SendAsync("NouveauRdv", rendezVous);
        }

        public async Task NotifierAnnulationAsync(Guid medecinId, Guid rendezVousId)
        {
            await _hubContext.Clients.Group(medecinId.ToString())
                .SendAsync("AnnulationRdv", rendezVousId);
        }
        // Hub SignalR pour Angular
        public class RendezVousHub : Hub { }
    }
}
