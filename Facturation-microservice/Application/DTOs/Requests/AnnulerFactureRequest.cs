namespace Facturation_microservice.Application.DTOs.Requests
{
    public class AnnulerFactureRequest
    {
        public Guid FactureId { get; set; }

        public string Motif { get; set; } = null!;
    }
}
