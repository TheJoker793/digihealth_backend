namespace Notification_microservice.Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        INotificationRepository Notifications { get; }
        ITemplateNotificationRepository Templates { get; }
        IPreferenceNotificationRepository Preferences { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
