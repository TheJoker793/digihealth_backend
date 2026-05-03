using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Rendez_vous_microservice.Infrastructure.Hubs
{
    public class AgendaHub : Hub
    {
        /// <summary>
        /// Lorsqu’un client se connecte, il rejoint le groupe correspondant à son cabinet.
        /// </summary>
        public async Task JoinCabinetGroup(Guid cabinetId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, cabinetId.ToString());
        }

        /// <summary>
        /// Lorsqu’un client se déconnecte, il quitte le groupe du cabinet.
        /// </summary>
        public async Task LeaveCabinetGroup(Guid cabinetId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, cabinetId.ToString());
        }

        /// <summary>
        /// 
        /// Notifie la création d’un nouveau rendez‑vous.
        /// </summary>
        public async Task NotifierNouveauRdv(Guid cabinetId, object rendezVous)
        {
            await Clients.Group(cabinetId.ToString())
                         .SendAsync("NouveauRdv", rendezVous);
        }

        /// <summary>
        /// Notifie l’annulation d’un rendez‑vous.
        /// </summary>
        public async Task NotifierRdvAnnule(Guid cabinetId, Guid rendezVousId)
        {
            await Clients.Group(cabinetId.ToString())
                         .SendAsync("RdvAnnule", rendezVousId);
        }
        /// <summary>
        /// Notifie la modification d’un rendez‑vous.
        /// </summary>
        public async Task NotifierRdvModifie(Guid cabinetId, object rendezVous)
        {
            await Clients.Group(cabinetId.ToString())
                         .SendAsync("RdvModifie", rendezVous);
        }
    }
}
