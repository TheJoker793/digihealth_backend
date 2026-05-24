using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Enums;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Application.DTOs.Requests;
using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Application.Services
{
    public class PrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPdfService _pdfService;
        private readonly INumeroPrescriptionGenerator _numeroGenerator;

        public PrescriptionService(IUnitOfWork unitOfWork, IPdfService pdfService, INumeroPrescriptionGenerator numeroGenerator)
        {
            _unitOfWork = unitOfWork;
            _pdfService = pdfService;
            _numeroGenerator = numeroGenerator;
        }

        // ── CREER ─────────────────────────────────────────────
        public async Task<Prescription> CreerAsync(CreerPrescriptionRequest request, CancellationToken ct = default)
        {
            var numero = await _numeroGenerator.GenerateAsync(request.CabinetId, ct);
            var numeroPrescription = NumeroPrescription.FromString(numero);

            var prescription = Prescription.Create(
                request.PatientId,
                request.MedecinId,
                request.CabinetId,
                numeroPrescription,
                request.TypePrescription,
                request.ValiditeJours,
                request.ConsultationId,
                request.DossierId,
                request.Instructions
            );

            await _unitOfWork.Prescriptions.AddAsync(prescription, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return prescription;
        }

        // ── VALIDER ───────────────────────────────────────────
        public async Task ValiderAsync(ValiderRequest request, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(request.PrescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            prescription.Valider();
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ── REFUSER ───────────────────────────────────────────
        public async Task RefuserAsync(RefuserRequest request, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(request.PrescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            prescription.Refuser(request.Motif);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ── ANNULER ───────────────────────────────────────────
        public async Task AnnulerAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            prescription.Annuler();
            await _unitOfWork.SaveChangesAsync(ct);
        }

        // ── GET BY ID ─────────────────────────────────────────
        public async Task<Prescription?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(id, ct);

        // ── HISTORIQUE ────────────────────────────────────────
        public async Task<IEnumerable<Prescription>> GetHistoriqueAsync(Guid patientId, CancellationToken ct = default)
            => await _unitOfWork.Prescriptions.GetByPatientAsync(patientId, ct);

        // ── ACTIVES ───────────────────────────────────────────
        public async Task<IEnumerable<Prescription>> GetActivesAsync(Guid patientId, CancellationToken ct = default)
            => await _unitOfWork.Prescriptions.GetActivesAsync(patientId, ct);

        // ── GENERER PDF ───────────────────────────────────────
        public async Task<byte[]> GenererPdfAsync(Guid prescriptionId, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            return await _pdfService.GenererPrescriptionAsync(prescription, ct);
        }

        public async Task<LignePrescription> AjouterLigneAsync(Guid prescriptionId, LignePrescription ligne, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            var newLigne = prescription.AddLigne(
                ligne.MedicamentId,
                ligne.NomMedicament,
                ligne.DCI,
                ligne.Posologie,
                ligne.DureeJours,
                ligne.Quantite,
                ligne.Renouvellement,
                ligne.NbRenouvellements,
                ligne.Commentaire
            );

            await _unitOfWork.SaveChangesAsync(ct);
            return newLigne;
        }

        public async Task SupprimerLigneAsync(Guid prescriptionId, Guid ligneId, CancellationToken ct = default)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionId, ct)
                ?? throw new InvalidOperationException("Prescription introuvable.");

            prescription.RemoveLigne(ligneId);
            await _unitOfWork.SaveChangesAsync(ct);
        }


    }
}
