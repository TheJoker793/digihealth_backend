namespace Facturation_microservice.Application.DTOs.Requests
{
    public class CreerFactureRequest
    {
        public Guid PatientId { get; set; }

        public Guid MedecinId { get; set; }

        public DateOnly DateEcheance { get; set; }

        public decimal TauxTVA { get; set; }

        public decimal Remise { get; set; }

        public List<AddLigneRequest> Lignes { get; set; } = new();
    }
}