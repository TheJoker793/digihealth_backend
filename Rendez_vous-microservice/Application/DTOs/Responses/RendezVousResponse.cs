using Rendez_vous_microservice.Domain.Enums;

namespace Rendez_vous_microservice.Application.DTOs.Responses
{
    public class RendezVousResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedecinId { get; set; }
        public DateTime DateHeure { get; set; }
        public int DureeMinutes { get; set; }
        public string Motif { get; set; } = string.Empty;
        public StatutRv Statut { get; set; }
        public TypeConsultation TypeConsultation { get; set; }
    }
}
