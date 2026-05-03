using System;

namespace Rendez_vous_microservice.Domain.Events
{
    public class RdvAnnuleEvent : BaseDomainEvent
    {
        public Guid RendezVousId { get; }
        public string Raison { get; }

        public RdvAnnuleEvent(Guid rendezVousId, string raison)
        {
            RendezVousId = rendezVousId;
            Raison = raison;
        }
    }
}
