namespace Notification_microservice.Domain.Interfaces
{
    public record SendResult(
    bool Succes,
    string? ProviderMessageId = null,
    string? Erreur = null);
}
