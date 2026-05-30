using Microsoft.EntityFrameworkCore.Storage;
using Notification_microservice.Domain.Interfaces;
using Notification_microservice.Infrastructure.Persistence.Repositories;

namespace Notification_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NotificationDbContext _context;
        private IDbContextTransaction? _transaction;
        public INotificationRepository Notifications { get; private set; }

        public ITemplateNotificationRepository Templates { get; private set; }

        public IPreferenceNotificationRepository Preferences { get; private set; }
        public UnitOfWork(NotificationDbContext context)
        {
            _context = context;
            Notifications = new NotificationRepository(_context);
            Templates = new TemplateNotificationRepository(_context);
            Preferences = new PreferenceNotificationRepository(_context);

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default)
            => _transaction = await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("Aucune transaction en cours.");

            await _context.SaveChangesAsync(ct);
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transaction is null) return;
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
    }
}
