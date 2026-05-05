using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Application.Services
{
    public class RendezVousService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RendezVousService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RendezVous> PrendreRdv(RendezVous rdv)
        {
            await _unitOfWork.RendezVous.AddAsync(rdv);
            await _unitOfWork.SaveChangesAsync();
            return rdv;
        }

        public async Task Confirmer(Guid rendezVousId)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            rdv.Confirmer();
            _unitOfWork.RendezVous.Update(rdv);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Annuler(Guid rendezVousId, string raison)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            rdv.Annuler(raison);
            _unitOfWork.RendezVous.Update(rdv);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Reporter(Guid rendezVousId, DateTime nouvelleDate)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            rdv.Reporter(nouvelleDate);
            _unitOfWork.RendezVous.Update(rdv);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task MarquerArrive(Guid rendezVousId)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            rdv.MarquerArrive();
            _unitOfWork.RendezVous.Update(rdv);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RendezVous>> GetAgenda(Guid medecinId, DateTime debut, DateTime fin)
        {
            return await _unitOfWork.RendezVous.GetAgendaAsync(medecinId, debut, fin);
        }

        public async Task<RendezVous?> GetById(Guid id)
        {
            return await _unitOfWork.RendezVous.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RendezVous>> GetByPatient(Guid patientId)
        {
            return await _unitOfWork.RendezVous.GetByPatientAsync(patientId);
        }

        public async Task Terminer(Guid rendezVousId)
        {
            var rdv = await _unitOfWork.RendezVous.GetByIdAsync(rendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable");
            rdv.TerminerConsultation(); // méthode métier déjà dans ton entité
            _unitOfWork.RendezVous.Update(rdv);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
