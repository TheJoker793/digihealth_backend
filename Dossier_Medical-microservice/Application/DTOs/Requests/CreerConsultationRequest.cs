using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public record CreerConsultationRequest
    (
         Guid DossierId,
         Guid? RendezVousId,
         TypeConsultation TypeConsultation,
         string Motif 
    );
}
