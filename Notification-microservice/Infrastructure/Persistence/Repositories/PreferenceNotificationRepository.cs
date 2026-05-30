using Microsoft.EntityFrameworkCore;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Persistence.Repositories
{
    public class PreferenceNotificationRepository
    : Repository<PreferenceNotification>, IPreferenceNotificationRepository
    {
        public PreferenceNotificationRepository(NotificationDbContext context) : base(context) { }

        public async Task<PreferenceNotification?> GetByDestinataireAsync(
            Guid destinataireId,
            string typeDestinataire,
            CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.DestinataireId == destinataireId
                      && p.TypeDestinataire == typeDestinataire, ct);

        public async Task<IEnumerable<PreferenceNotification>> GetOptOutsAsync(
            CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(p => p.EstOptOut)
                .ToListAsync(ct);
    }
}
