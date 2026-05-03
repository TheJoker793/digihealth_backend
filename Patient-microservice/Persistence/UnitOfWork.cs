using Microsoft.EntityFrameworkCore.Storage;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Repositories;

namespace Patient_microservice.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PatientDbContext _context;
        private IDbContextTransaction? _transaction;

        public IPatientRepository Patients { get; }
        public IAntecedentRepository Antecedents { get; }

        public IAssuranceComplementaireRepository AssuranceComplementaires { get; }

        public IContactUrgenceRepository ContactUrgences { get; }

        public ICouvertureSocialeRepository CouvertureSociales { get; }

        public IPieceIdentiteRepository PieceIdentites { get; }

        public UnitOfWork(PatientDbContext context)
        {
            _context = context;

            Patients = new PatientRepository(_context);
            Antecedents = new AntecedentRepository(_context);
            AssuranceComplementaires = new AssuranceComplementaireRepository(_context);
            ContactUrgences = new ContactUrgenceRepository(_context);
            CouvertureSociales = new CouvertureSocialeRepository(_context);
            PieceIdentites = new PieceIdentiteRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null) return;

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
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
                await RollbackAsync();
                throw;
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