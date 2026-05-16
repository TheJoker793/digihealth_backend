using Dossier_Medical_microservice.Domain.Enums;

namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record ConsultationResponse(
        Guid Id,
        Guid DossierId,
        DateTimeOffset Date,
        TypeConsultation TypeConsultation,
        string Motif,
        StatutConsultation Statut,
        ExamenCliniqueResponse ExamenClinique,
        string? Conclusion,
        IEnumerable<DiagnosticResponse> Diagnostics,
        IEnumerable<OrdonnanceResponse> Ordonnances
    );
}
