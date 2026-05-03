namespace Patient_microservice.Domain.Entities
{
    public class AssuranceComplementaire : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        public string NomAssureur { get; set; }
        public string NumeroPolice { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }
        public decimal TauxRemboursement { get; set; }
        public string? Telephone { get; set; }
    }
}
