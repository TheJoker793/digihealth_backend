using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Document_microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocumentDbContext _context;
        private IDbContextTransaction? _transaction;

        public IDocumentRepository Documents { get; }
        public IVersionDocumentRepository Versions { get; }
        public IPartageDocumentRepository Partages { get; }
        public ITemplateDocumentRepository Templates { get; }

        public UnitOfWork(DocumentDbContext context)
        {
            _context = context;

            Documents = new DocumentRepository(_context);
            Versions = new VersionDocumentRepository(_context);
            Partages = new PartageDocumentRepository(_context);
            Templates = new TemplateDocumentRepository(_context);
        }

        // ─────────────────────────────────────────────
        // Transactions
        // ─────────────────────────────────────────────

        public async Task BeginTransactionAsync(
            CancellationToken ct = default)
        {
            if (_transaction != null)
                return;

            _transaction = await _context.Database
                .BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(
            CancellationToken ct = default)
        {
            try
            {
                await _context.SaveChangesAsync(ct);

                if (_transaction != null)
                    await _transaction.CommitAsync(ct);
            }
            catch
            {
                await RollbackAsync(ct);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync(
            CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();

                _transaction = null;
            }
        }

        // ─────────────────────────────────────────────
        // Persistence
        // ─────────────────────────────────────────────

        public async Task<int> SaveChangesAsync(
            CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        // ─────────────────────────────────────────────
        // Dispose
        // ─────────────────────────────────────────────

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}