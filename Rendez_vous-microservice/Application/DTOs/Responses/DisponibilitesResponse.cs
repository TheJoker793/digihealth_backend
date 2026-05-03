namespace Rendez_vous_microservice.Application.DTOs.Responses
{
    public class DisponibilitesResponse
    {
        public Guid MedecinId { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public IEnumerable<CreneauResponse> CreneauxDisponibles { get; set; } = new List<CreneauResponse>();
    }
}
