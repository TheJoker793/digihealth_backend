using Rendez_vous_microservice.Domain.Enums;

namespace Rendez_vous_microservice.Application.DTOs.Responses
{
    public class CreneauResponse
    {
        public Guid Id { get; set; }
        public Guid MedecinId { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public bool EstDisponible { get; set; }
        public TypeCreneau TypeCreneau { get; set; }
    }
}
