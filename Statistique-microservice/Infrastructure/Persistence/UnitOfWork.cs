using Microsoft.EntityFrameworkCore.Storage;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Infrastructure.Persistence.Repositories;

namespace Statistique_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StatistiqueDbContext _context;
        private IDbContextTransaction? _transaction;

        private IRapportRepository? _rapports;
        private IKPIRepository? _kpis;
        private ISnapshotRepository? _snapshots;
        private ITableauDeBordRepository? _tableaux;
        private IAbonnementRepository? _abonnements;

        public UnitOfWork(StatistiqueDbContext context) => _context = context;

        public IRapportRepository Rapports => _rapports ??= new RapportRepository(_context);
        public IKPIRepository KPIs => _kpis ??= new KPIRepository(_context);
        public ISnapshotRepository Snapshots => _snapshots ??= new SnapshotRepository(_context);
        public ITableauDeBordRepository Tableaux => _tableaux ??= new TableauDeBordRepository(_context);
        public IAbonnementRepository Abonnements => _abonnements ??= new AbonnementRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default)
            => _transaction = await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
            await _transaction!.CommitAsync(ct);
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
            if (_transaction is not null) await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
    }
}
