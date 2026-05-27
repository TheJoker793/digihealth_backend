namespace Facturation_microservice.Application.DTOs.Responses
{
    public class FactureStatsResponse
    {
        public int NombreFactures { get; set; }

        public decimal TotalFacture { get; set; }

        public decimal TotalPaye { get; set; }
    }
}
