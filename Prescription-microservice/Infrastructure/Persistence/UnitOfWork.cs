using Microsoft.EntityFrameworkCore.Storage;
using Prescription_microservice.Domain.Interfaces;

namespace Prescription_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PrescriptionDbContext _context;
        private IDbContextTransaction? _transaction;

        public IPrescriptionRepository Prescriptions { get; }
        public ILignePrescriptionRepository Lignes { get; }
        public IInteractionRepository Interactions { get; }

        public UnitOfWork(
            PrescriptionDbContext context,
            IPrescriptionRepository prescriptions,
            ILignePrescriptionRepository lignes,
            IInteractionRepository interactions)
        {
            _context = context;
            Prescriptions = prescriptions;
            Lignes = lignes;
            Interactions = interactions;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
        }
    }
}
