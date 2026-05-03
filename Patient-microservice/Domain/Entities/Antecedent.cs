using Patient_microservice.Domain.Enums;

namespace Patient_microservice.Domain.Entities
{
    public class Antecedent:BaseEntity
    {
        public TypeAntecedent TypeAntecedent { get; set; }
        public string? Description { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }
        public bool EstActif { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string? Substance { get; set; }
        public Severite Severite { get; set; }
        public string? TypeReaction { get; set; }




    }
}
