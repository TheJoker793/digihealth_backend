using Auth_microservice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth_microservice.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; }
        public ICabinetRepository Cabinets { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(
            AuthDbContext context,
            IUserRepository userRepository,
            ICabinetRepository cabinetRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;
            Users = userRepository;
            Cabinets = cabinetRepository;
            RefreshTokens = refreshTokenRepository;
        }

        // =========================
        // SAVE CHANGES
        // =========================
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        // =========================
        // TRANSACTION
        // =========================
        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
                return;

            _transaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
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

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
