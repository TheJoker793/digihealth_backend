using System;

namespace Rendez_vous_microservice.Domain.Events
{
    public class RappelRdvEvent : BaseDomainEvent
    {
        public Guid RendezVousId { get; }
        public Guid PatientId { get; }
        public DateTime DateRappel { get; }

        public RappelRdvEvent(Guid rendezVousId, Guid patientId, DateTime dateRappel)
        {
            RendezVousId = rendezVousId;
            PatientId = patientId;
            DateRappel = dateRappel;
        }
    }
}
