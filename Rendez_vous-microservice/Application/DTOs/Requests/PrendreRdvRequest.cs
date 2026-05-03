using Rendez_vous_microservice.Domain.Enums;

namespace Rendez_vous_microservice.Application.DTOs.Requests
{
    public class PrendreRdvRequest
    {
        public Guid PatientId { get; set; }
        public Guid MedecinId { get; set; }
        public Guid CabinetId { get; set; }
        public DateTime DateHeure { get; set; }
        public int DureeMinutes { get; set; }
        public string Motif { get; set; } = string.Empty;
        public TypeConsultation TypeConsultation { get; set; }
    }
}
