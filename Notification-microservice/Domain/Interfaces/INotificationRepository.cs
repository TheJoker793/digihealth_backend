using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Domain.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<Notification?> GetAvecHistoriquesAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<Notification>> GetEnAttenteAsync(
            int limite = 50, CancellationToken ct = default);

        Task<IEnumerable<Notification>> GetEchoueesAsync(
            int maxTentatives = 3, CancellationToken ct = default);

        Task<IEnumerable<Notification>> GetByDestinataireAsync(
            Guid destinataireId, CancellationToken ct = default);

        Task<bool> NumeroExisteAsync(string numero, CancellationToken ct = default);
    }
}
