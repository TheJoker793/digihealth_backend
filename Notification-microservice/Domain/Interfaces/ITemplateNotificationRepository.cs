using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Domain.Interfaces
{
    public interface ITemplateNotificationRepository : IRepository<TemplateNotification>
    {
        Task<TemplateNotification?> GetByCodeAsync(
            string code, CancellationToken ct = default);

        Task<TemplateNotification?> GetByEvenementEtCanalAsync(
            TypeEvenement typeEvenement,
            CanalEnvoi canal,
            string langue = "fr",
            CancellationToken ct = default);

        Task<IEnumerable<TemplateNotification>> GetByEvenementAsync(
            TypeEvenement typeEvenement,
            CancellationToken ct = default);
    }
}
