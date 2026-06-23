using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dossier_Medical_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DossierDbContext _context;
        private IDbContextTransaction? _transaction;

        public IDossierRepository Dossiers { get; }
        public IConsultationRepository Consultations { get; }
        public IDiagnosticRepository Diagnostics { get; }
        public IOrdonnanceRepository Ordonnances { get; }
        public IDocumentRepository Documents { get; }

        public UnitOfWork(DossierDbContext context)  // ✅ plus d'IDbContextTransaction
        {
            _context = context;
            Dossiers = new DossierRepository(_context);
            Consultations = new ConsultationRepository(_context);
            Diagnostics = new DiagnosticRepository(_context);
            Ordonnances = new OrdonnanceRepository(_context);
            Documents = new DocumentRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null) return;
            _transaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_transaction == null) return;
            await _context.SaveChangesAsync(ct);
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transaction == null) return;
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}