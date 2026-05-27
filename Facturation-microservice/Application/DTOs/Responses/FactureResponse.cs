namespace Facturation_microservice.Application.DTOs.Responses
{
    public class FactureResponse
    {
        public Guid Id { get; set; }

        public string NumeroFacture { get; set; } = null!;

        public decimal MontantTTC { get; set; }

        public decimal MontantPaye { get; set; }

        public decimal ResteAPayer { get; set; }
    }
}
