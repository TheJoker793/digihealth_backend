using Facturation_microservice.Domain.Enums;

namespace Facturation_microservice.Application.DTOs.Requests
{
    public class EnregistrerPaiementRequest
    {
        public Guid FactureId { get; set; }

        public decimal Montant { get; set; }

        public ModeReglement Mode { get; set; }

        public Guid Caissier { get; set; }

        public string? Reference { get; set; }
    }
}
