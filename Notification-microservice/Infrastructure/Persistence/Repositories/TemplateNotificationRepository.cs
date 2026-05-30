using Microsoft.EntityFrameworkCore;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Persistence.Repositories
{
    public class TemplateNotificationRepository
    : Repository<TemplateNotification>, ITemplateNotificationRepository
    {
        public TemplateNotificationRepository(NotificationDbContext context) : base(context) { }

        public async Task<TemplateNotification?> GetByCodeAsync(
            string code, CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Code == code.ToLowerInvariant(), ct);

        public async Task<TemplateNotification?> GetByEvenementEtCanalAsync(
            TypeEvenement typeEvenement,
            CanalEnvoi canal,
            string langue = "fr",
            CancellationToken ct = default)
        {
            // Cherche d'abord la langue exacte, sinon fallback sur "fr"
            return await _dbSet
                .AsNoTracking()
                .Where(t => t.TypeEvenement == typeEvenement
                         && t.Canal == canal
                         && t.EstActif)
                .OrderByDescending(t => t.Langue == langue)   // langue exacte en premier
                .ThenByDescending(t => t.Langue == "fr")       // fallback fr
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<TemplateNotification>> GetByEvenementAsync(
            TypeEvenement typeEvenement, CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(t => t.TypeEvenement == typeEvenement)
                .OrderBy(t => t.Canal)
                .ThenBy(t => t.Langue)
                .ToListAsync(ct);
    }
}
