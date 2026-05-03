namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class ReporterRdvRequest
    {
        public Guid RendezVousId { get; set; }
        public DateTime NouvelleDate { get; set; }
    }
}
