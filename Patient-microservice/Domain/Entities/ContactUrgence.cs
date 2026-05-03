using Patient_microservice.Domain.Enums;

namespace Patient_microservice.Domain.Entities
{
    public class ContactUrgence:BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public LienParente LienParente { get; set; }
        public string Telephone { get; set; }
    }
}
