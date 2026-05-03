using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Rendez_vous_microservice.Domain.Interfaces;
using Rendez_vous_microservice.Infrastructure.Persistence;
using Rendez_vous_microservice.Infrastructure.Persistence.Repositories;

namespace DMI.RendezVous.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RendezVousDbContext _context;
        private IDbContextTransaction? _transaction;

        public IRendezVousRepository RendezVous { get; }
        public ICreneauRepository Creneaux { get; }
        public IRegleRecurrenceRepository Recurrences { get; }

        public UnitOfWork(RendezVousDbContext context)
        {
            _context = context;

            RendezVous = new RendezVousRepository(_context);
            Creneaux = new CreneauRepository(_context);
            Recurrences = new RegleRecurrenceRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null) return; // éviter double ouverture
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
