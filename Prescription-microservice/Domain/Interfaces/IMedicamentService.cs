namespace Prescription_microservice.Domain.Interfaces
{
    public interface IMedicamentService
    {
        Task<bool> ExistsAsync(Guid medicamentId);

        Task<IEnumerable<Guid>> GetExistingIdsAsync(
            IEnumerable<Guid> medicamentIds);

        Task<IEnumerable<(Guid MedicamentA, Guid MedicamentB, string Severite)>>
            CheckInteractionsAsync(IEnumerable<Guid> medicamentIds);
    }
}
