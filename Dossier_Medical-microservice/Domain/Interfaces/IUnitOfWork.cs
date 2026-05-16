
namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IDossierRepository Dossiers { get; }
        IConsultationRepository Consultations { get; }
        IDiagnosticRepository Diagnostics { get; }
        IOrdonnanceRepository Ordonnances { get; }
        IDocumentRepository Documents { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
