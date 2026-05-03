namespace Rendez_vous_microservice.Application.DTOs.Responses
{
    public class AgendaResponse
    {
        public Guid MedecinId { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public IEnumerable<RendezVousSummaryResponse> RendezVous { get; set; } = new List<RendezVousSummaryResponse>();
    }
}
