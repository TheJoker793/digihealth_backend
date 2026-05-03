using Patient_microservice.Domain.Enums;

namespace Patient_microservice.Domain.Entities
{
    public class CouvertureSociale:BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public TypeCaisse TypeCaisse { get; set; }
        public string NumeroAffilie { get; set; }
        public string? NumeroImmatriculation { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }
        public decimal TauxPriseEnCharge { get; set; }
        public bool EstALD { get; set; }
        public string? CodeAld { get; set; }







    }
}
