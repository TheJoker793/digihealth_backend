using Facturation_microservice.Domain.Interfaces;
using Facturation_microservice.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Facturation_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FacturationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IFactureRepository Factures { get; private set; }

        public IPaiementRepository Paiements { get; private set; }

        public IRemboursementRepository Remboursements { get; private set; }

        public ILigneFactureRepository LignesFacture { get; private set; }

        public UnitOfWork(FacturationDbContext context)
        {
            _context = context;

            Factures = new FactureRepository(_context);
            Paiements = new PaiementRepository(_context);
            Remboursements = new RemboursementRepository(_context);
            LignesFacture = new LigneFactureRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
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