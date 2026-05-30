namespace Notification_microservice.Domain.Interfaces
{
    public interface ITemplateRenderer
    {
        Task<string> RendreAsync(
            string template,
            Dictionary<string, string> variables,
            CancellationToken ct = default);
    }
}
