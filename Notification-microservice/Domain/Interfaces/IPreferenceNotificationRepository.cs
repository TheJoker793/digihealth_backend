using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Domain.Interfaces
{
    public interface IPreferenceNotificationRepository : IRepository<PreferenceNotification>
    {
        Task<PreferenceNotification?> GetByDestinataireAsync(
            Guid destinataireId,
            string typeDestinataire,
            CancellationToken ct = default);

        Task<IEnumerable<PreferenceNotification>> GetOptOutsAsync(
            CancellationToken ct = default);
    }
}
