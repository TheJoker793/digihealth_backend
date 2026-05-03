namespace Rendez_vous_microservice.Application.DTOs.Responses
{
    public class RendezVousSummaryResponse
    {
        public Guid Id { get; set; }
        public DateTime DateHeure { get; set; }
        public string MedecinNom { get; set; } = string.Empty;
        public string Motif { get; set; } = string.Empty;
    }
}
