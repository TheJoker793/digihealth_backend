using Patient_microservice.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    IAntecedentRepository Antecedents { get; }
    IAssuranceComplementaireRepository AssuranceComplementaires { get; }
    IContactUrgenceRepository ContactUrgences { get; }
    ICouvertureSocialeRepository CouvertureSociales { get; }
    IPieceIdentiteRepository PieceIdentites { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}