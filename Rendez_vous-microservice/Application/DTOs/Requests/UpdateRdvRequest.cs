namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class UpdateRdvRequest
    {
        public Guid RendezVousId { get; set; }
        public DateTime NouvelleDate { get; set; }
        public int NouvelleDureeMinutes { get; set; }
        public string? NouveauMotif { get; set; }
    }
}
