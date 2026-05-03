namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class AnnulerRdvRequest
    {
        public Guid RendezVousId { get; set; }
        public string Raison { get; set; } = string.Empty;
    }
}
