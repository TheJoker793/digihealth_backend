using Dossier_Medical_microservice.Domain.Entities;

namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IConsultationRepository:IRepository<Consultation>
    {
        Task<IEnumerable<Consultation>> GetByDossierAsync(Guid dossierId, CancellationToken ct = default);
        Task<IEnumerable<Consultation>> GetHistoriqueAsync(Guid dossierId, CancellationToken ct = default);

    }
}
