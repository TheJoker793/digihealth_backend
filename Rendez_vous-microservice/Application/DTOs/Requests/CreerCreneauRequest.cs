using Rendez_vous_microservice.Domain.Enums;

namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class CreerCreneauRequest
    {
        public Guid MedecinId { get; set; }
        public Guid CabinetId { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public TypeCreneau TypeCreneau { get; set; }
    }
}
