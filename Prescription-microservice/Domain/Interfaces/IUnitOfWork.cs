using Microsoft.EntityFrameworkCore.Storage;

namespace Prescription_microservice.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPrescriptionRepository Prescriptions { get; }
        ILignePrescriptionRepository Lignes { get; }
        IInteractionRepository Interactions { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }

}
