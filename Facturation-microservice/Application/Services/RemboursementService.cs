using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Interfaces;

namespace Facturation_microservice.Application.Services
{
    public class RemboursementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemboursementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CalculerRemboursementAsync(Guid factureId)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId);


            var remboursement = new RemboursementCaisse();

            remboursement.Calculer(new List<LigneFacture>());

            await _unitOfWork.Remboursements.AddAsync(remboursement);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SoumettreALaCaisse(Guid factureId)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId);

            if (facture == null)
                throw new Exception("Facture introuvable");

            var remboursement = new RemboursementCaisse
            {
                FactureId = factureId,
                Statut = Domain.Enums.StatutRemboursement.Soumis
            };

            remboursement.Calculer(new List<LigneFacture>());

            await _unitOfWork.Remboursements.AddAsync(remboursement);
            await _unitOfWork.SaveChangesAsync();
        }

       
        
        public async Task<IEnumerable<RemboursementCaisse>> GetByFactureAsync(Guid factureId)
        {
            return await _unitOfWork.Remboursements.GetByFactureAsync(factureId);
        }

    }
}
