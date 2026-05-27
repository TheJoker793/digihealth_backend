using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Application.DTOs.Requests
{
    public class SoumettreRemboursementRequest
    {
        public Guid FactureId { get; set; }

        public string NumeroAffilie { get; set; } = null!;

        public ModeReglement Mode { get; set; }
    }
}
