using Document_microservice.Domain.Entities;
using Document_microservice.Domain.Enums;

namespace Document_microservice.Domain.Interfaces
{
    public interface IDocumentRepository : IRepository<DocumentMedical>
    {
        Task<DocumentMedical?> GetAvecVersionsAsync(Guid id, CancellationToken ct = default);

        Task<DocumentMedical?> GetAvecPartagesAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<DocumentMedical>> GetByPatientAsync(
            Guid patientId,
            CancellationToken ct = default);

        Task<IEnumerable<DocumentMedical>> GetByCabinetAsync(
            Guid cabinetId,
            TypeDocument? type = null,
            StatutDocument? statut = null,
            CancellationToken ct = default);

        Task<IEnumerable<DocumentMedical>> GetByConsultationAsync(
            Guid consultationId,
            CancellationToken ct = default);

        Task<bool> NumeroExisteAsync(string numero, CancellationToken ct = default);

    }
}
