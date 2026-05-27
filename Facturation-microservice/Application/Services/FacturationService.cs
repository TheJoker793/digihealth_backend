using Facturation_microservice.Application.DTOs.Requests;
using Facturation_microservice.Application.DTOs.Responses;
using Facturation_microservice.Domain.Entities;
using Facturation_microservice.Domain.Enums;
using Facturation_microservice.Domain.Events;
using Facturation_microservice.Domain.Interfaces;
using static Facturation_microservice.Application.Exceptions.AppExceptions;

namespace Facturation_microservice.Application.Services
{
    public class FacturationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _publisher;
        private readonly INumeroFactureGenerator _numeroGenerator;
        private readonly IPdfService _pdfService;
        public FacturationService(IUnitOfWork unitOfWork,IEventPublisher eventPublisher,INumeroFactureGenerator numeroFacture,IPdfService pdfService)
        {
            _unitOfWork = unitOfWork;
            _publisher = eventPublisher;
            _numeroGenerator = numeroFacture;
            _pdfService = pdfService;
            
        }

        public async Task<Guid> CreerFactureAsync(CreerFactureRequest request)
        {
            var numero = await _numeroGenerator.GenerateAsync(request.PatientId);

            var facture = Facture.Create(
                numero,
                request.PatientId,
                request.MedecinId,
                request.DateEcheance,
                request.DateEcheance,
                request.TauxTVA,
                request.Remise
            );

            await _unitOfWork.Factures.AddAsync(facture);
            await _unitOfWork.SaveChangesAsync();

            return facture.Id;
        }
        public async Task ValiderAsync(Guid factureId)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId)
                ?? throw new NotFoundException("Facture introuvable");

            facture.Valider();

            await _unitOfWork.SaveChangesAsync();

            await _publisher.PublishAsync(
                new FactureValideeEvent(facture.Id, facture.PatientId)
            );
        }

        public async Task EnregistrerPaiementAsync(EnregistrerPaiementRequest request)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(request.FactureId)
                ?? throw new NotFoundException("Facture introuvable");

            if (facture.Statut == StatutFacture.Annulee)
                throw new FactureAnnuleeException();

            facture.Payer(request.Mode, request.Montant);

            var paiement = Paiement.Create(
                request.FactureId,
                request.Montant,
                request.Mode,
                request.Caissier,
                request.Reference
            );

            await _unitOfWork.Paiements.AddAsync(paiement);

            await _unitOfWork.SaveChangesAsync();

            await _publisher.PublishAsync(
                new PaiementEnregistreEvent(
                    facture.Id,
                    request.Montant,
                    request.Mode
                )
            );

            if (facture.Statut == StatutFacture.Payee)
            {
                await _publisher.PublishAsync(
                    new FacturePayeeEvent(facture.Id, facture.PatientId)
                );
            }
        }

        public async Task AnnulerAsync(AnnulerFactureRequest request)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(request.FactureId)
                ?? throw new NotFoundException("Facture introuvable");

            facture.Annuler(request.Motif);

            await _unitOfWork.SaveChangesAsync();

            await _publisher.PublishAsync(
                new FactureAnnuleeEvent(facture.Id)
            );
        }

        public async Task<FactureResponse> GetByIdAsync(Guid id)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(id)
                ?? throw new NotFoundException("Facture introuvable");

            return new FactureResponse
            {
                Id = facture.Id,
                NumeroFacture = facture.NumeroFacture,
                MontantTTC = facture.MontantTTC(),
                MontantPaye = 0,
                ResteAPayer = 0
            };
        }

        public async Task<byte[]> GenererPdfAsync(Guid factureId)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId)
                ?? throw new NotFoundException("Facture introuvable");

            return await _pdfService.GenererFacturePdfAsync(facture);
        }

        public async Task<object> GetStatsAsync()
        {
            return await _unitOfWork.Factures.GetStatsAsync();
        }

        public async Task<IEnumerable<PaiementResponse>> GetHistoriquePaiementsAsync(Guid factureId)
        {
            var paiements = await _unitOfWork.Paiements.GetByFactureAsync(factureId);

            return paiements.Select(p => new PaiementResponse
            {
                Montant = p.Montant,
                Mode = p.Mode.ToString(),
                Date = p.Date
            });
        }


        public async Task AddLigne(Guid factureId, AddLigneRequest request)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId)
                ?? throw new NotFoundException("Facture introuvable");

            var ligne = new LigneFacture
            {
                FactureId = factureId,
                Designation = request.Designation,
                CodeActe = request.CodeActe,
                PrixUnitaire = request.PrixUnitaire,
                Quantite = request.Quantite,
                TauxRemboursement = request.TauxRemboursement
            };

            await _unitOfWork.LignesFacture.AddAsync(ligne);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task RemoveLigne(Guid factureId, Guid ligneId)
        {
            var facture = await _unitOfWork.Factures.GetByIdAsync(factureId)
                ?? throw new NotFoundException("Facture introuvable");

            var ligne = await _unitOfWork.LignesFacture.GetByIdAsync(ligneId)
                ?? throw new NotFoundException("Ligne introuvable");

            if (ligne.FactureId != factureId)
                throw new Exception("Cette ligne n'appartient pas à cette facture");

            _unitOfWork.LignesFacture.Delete(ligne);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
