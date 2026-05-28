namespace Document_microservice.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IDocumentRepository Documents { get; }
        IVersionDocumentRepository Versions { get; }
        IPartageDocumentRepository Partages { get; }
        ITemplateDocumentRepository Templates { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>Démarre une transaction explicite (opérations multi-agrégats).</summary>
        Task BeginTransactionAsync(CancellationToken ct = default);

        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
