namespace Facturation_microservice.Application.DTOs.Requests
{
    public class AddLigneRequest
    {
        public string Designation { get; set; } = null!;

        public decimal PrixUnitaire { get; set; }

        public int Quantite { get; set; }

        public decimal TauxRemboursement { get; set; }

        public string? CodeActe { get; set; }
    }
}