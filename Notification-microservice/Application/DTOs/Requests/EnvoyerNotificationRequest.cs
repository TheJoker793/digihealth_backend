using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.DTOs.Requests
{
    public record EnvoyerNotificationRequest(
    TypeEvenement TypeEvenement,
    Guid DestinataireId,
    string TypeDestinataire,            // "Patient" | "Medecin"
    CanalEnvoi Canal,
    Dictionary<string, string> Variables,
    Guid? SourceId = null,              // RdvId, DocumentId…
    string? ContactEmail = null,
    string? ContactTelephone = null,
    string? TokenFcm = null,
    string? PieceJointeChemin = null,
    DateTimeOffset? DateProgrammee = null
);
}
