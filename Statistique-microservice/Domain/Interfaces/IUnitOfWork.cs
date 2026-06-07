namespace Statistique_microservice.Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRapportRepository Rapports { get; }
        IKPIRepository KPIs { get; }
        ISnapshotRepository Snapshots { get; }
        ITableauDeBordRepository Tableaux { get; }
        IAbonnementRepository Abonnements { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
