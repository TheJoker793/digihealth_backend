using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_Vous_microservice.Application.Services
{
    public class RecurrenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecurrenceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // 🔹 Créer une règle récurrente
        public async Task<RegleRecurrence> CreerRecurrence(RegleRecurrence regle)
        {
            await _unitOfWork.Recurrences.AddAsync(regle);
            await _unitOfWork.SaveChangesAsync();
            return regle;
        }

        // 🔹 Récupérer les règles actives d’un médecin
        public async Task<IEnumerable<RegleRecurrence>> GetByMedecin(Guid medecinId)
        {
            return await _unitOfWork.Recurrences.GetByMedecinAsync(medecinId);
        }

        // 🔹 Supprimer une règle
        public async Task Supprimer(Guid id)
        {
            var regle = await _unitOfWork.Recurrences.GetByIdAsync(id)
                         ?? throw new Exception("Règle de récurrence introuvable");
            _unitOfWork.Recurrences.Remove(regle);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
