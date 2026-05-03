using System;

namespace Rendez_vous_microservice.Domain.Events
{
    public class RdvConfirmeEvent : BaseDomainEvent
    {
        public Guid RendezVousId { get; }
        public Guid PatientId { get; }

        public RdvConfirmeEvent(Guid rendezVousId, Guid patientId)
        {
            RendezVousId = rendezVousId;
            PatientId = patientId;
        }
    }
}
