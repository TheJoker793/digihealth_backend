using Rendez_vous_microservice.Domain.Entities;
using Rendez_vous_microservice.Domain.Enums;
using Rendez_vous_microservice.Domain.Interfaces;

namespace Rendez_vous_microservice.Application.Services
{
    public class CreneauService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreneauService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Creneau>> GetDisponibles(Guid medecinId, DateTime debut, DateTime fin)
        {
            return await _unitOfWork.Creneaux.GetDisponiblesAsync(medecinId, debut, fin);
        }

        public async Task<Creneau> CreerCreneau(Creneau creneau)
        {
            await _unitOfWork.Creneaux.AddAsync(creneau);
            await _unitOfWork.SaveChangesAsync();
            return creneau;
        }
        public async Task Bloquer(Guid creneauId)
        {
            var creneau = await _unitOfWork.Creneaux.GetByIdAsync(creneauId)
                          ?? throw new Exception("Créneau introuvable");
            creneau.Bloquer();
            _unitOfWork.Creneaux.Update(creneau);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<Creneau>> GenererRecurrence(RegleRecurrence regle, Guid cabinetId, TypeCreneau type)
        {
            var creneaux = regle.GenererCreneaux(cabinetId, type);
            foreach (var c in creneaux)
                await _unitOfWork.Creneaux.AddAsync(c);

            await _unitOfWork.SaveChangesAsync();
            return creneaux;
        }

        public async Task<bool> VerifierDisponibilite(Guid medecinId, DateTime debut, DateTime fin)
        {
            return !await _unitOfWork.Creneaux.ExisteChevaucheAsync(medecinId, debut, fin);
        }
    }
}
