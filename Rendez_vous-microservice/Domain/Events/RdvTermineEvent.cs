using System;

namespace Rendez_vous_microservice.Domain.Events
{
    public class RdvTermineEvent : BaseDomainEvent
    {
        public Guid RendezVousId { get; }
        public Guid PatientId { get; }

        public RdvTermineEvent(Guid rendezVousId, Guid patientId)
        {
            RendezVousId = rendezVousId;
            PatientId = patientId;
        }
    }
}
