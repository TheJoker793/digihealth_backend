namespace Facturation_microservice.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IFactureRepository Factures { get; }

        ILigneFactureRepository LignesFacture { get; }
        IPaiementRepository Paiements { get; }

        IRemboursementRepository Remboursements { get; }


        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}
