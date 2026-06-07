using Auth_microservice.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICabinetRepository Cabinets { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}