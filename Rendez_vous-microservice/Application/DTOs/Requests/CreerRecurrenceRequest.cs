namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class CreerRecurrenceRequest
    {
        public Guid MedecinId { get; set; }
        public int JoursSemaineBits { get; set; }
        public TimeOnly HeureDebut { get; set; }
        public TimeOnly HeureFin { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }
    }
}
