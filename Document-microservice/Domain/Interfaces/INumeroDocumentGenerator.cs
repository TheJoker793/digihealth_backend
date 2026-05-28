namespace Document_microservice.Domain.Interfaces
{
    public interface INumeroDocumentGenerator
    {
        Task<string> GenererAsync(Guid cabinetId, CancellationToken ct = default);

    }
}
