using Patient_microservice.Domain.Enums;

namespace Patient_microservice.Domain.Entities
{
    public class PieceIdentite:BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public TypePiece TypePiece { get; set; }
        public  string? Numero { get;set; }
        public DateOnly? DateExpiration { get; set; }
        public required string PaysEmetteur { get; set; }
        public bool EstPrincipal { get; set; }






    }
}
