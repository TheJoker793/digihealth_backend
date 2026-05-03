using System;

namespace Rendez_vous_microservice.Domain.Events
{
    public class RdvReporteEvent : BaseDomainEvent
    {
        public Guid RendezVousId { get; }
        public DateTime AncienneDate { get; }
        public DateTime NouvelleDate { get; }

        public RdvReporteEvent(Guid rendezVousId, DateTime ancienneDate, DateTime nouvelleDate)
        {
            RendezVousId = rendezVousId;
            AncienneDate = ancienneDate;
            NouvelleDate = nouvelleDate;
        }
    }
}
