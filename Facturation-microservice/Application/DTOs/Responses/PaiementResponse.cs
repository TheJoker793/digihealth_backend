namespace Facturation_microservice.Application.DTOs.Responses
{
    public class PaiementResponse
    {
        public decimal Montant { get; set; }

        public string Mode { get; set; } = null!;

        public DateTimeOffset Date { get; set; }
    }
}
