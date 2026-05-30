using Microsoft.EntityFrameworkCore;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;
using Notification_microservice.Domain.Interfaces;

namespace Notification_microservice.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository
    : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(NotificationDbContext context) : base(context) { }

        public async Task<Notification?> GetAvecHistoriquesAsync(
            Guid id, CancellationToken ct = default)
            => await _dbSet
                .Include(n => n.Historiques.OrderByDescending(h => h.DateTentative))
                .FirstOrDefaultAsync(n => n.Id == id, ct);

        public async Task<IEnumerable<Notification>> GetEnAttenteAsync(
            int limite = 50, CancellationToken ct = default)
            => await _dbSet
                .Where(n => n.Statut == StatutNotification.EnAttente
                         && (n.DateProgrammee == null
                             || n.DateProgrammee <= DateTimeOffset.UtcNow))
                .OrderBy(n => n.CreatedAt)
                .Take(limite)
                .ToListAsync(ct);

        public async Task<IEnumerable<Notification>> GetEchoueesAsync(
            int maxTentatives = 3, CancellationToken ct = default)
            => await _dbSet
                .Where(n => n.Statut == StatutNotification.Echoue
                         && n.NbTentatives < maxTentatives)
                .OrderBy(n => n.CreatedAt)
                .ToListAsync(ct);

        public async Task<IEnumerable<Notification>> GetByDestinataireAsync(
            Guid destinataireId, CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(n => n.DestinataireId == destinataireId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(100)
                .ToListAsync(ct);

        public async Task<bool> NumeroExisteAsync(
            string numero, CancellationToken ct = default)
            => await _dbSet.AnyAsync(n => n.Numero == numero, ct);
    }

}
