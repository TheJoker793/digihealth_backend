namespace Facturation_microservice.Application.DTOs.Responses
{
    public class LigneFactureResponse
    {
        public string Designation { get; set; } = null!;

        public decimal PrixUnitaire { get; set; }

        public int Quantite { get; set; }

        public decimal MontantHT { get; set; }
    }
}
