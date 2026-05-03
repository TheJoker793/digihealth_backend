using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rendez_vous_microservice.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRendezVousRepository RendezVous { get; }
        ICreneauRepository Creneaux { get; }
        IRegleRecurrenceRepository Recurrences { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
