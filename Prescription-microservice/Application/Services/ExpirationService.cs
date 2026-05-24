using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;

namespace Prescription_microservice.Application.Services
{
    public class ExpirationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpirationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ── CHECK EXPIREES ────────────────────────────────────
        public async Task CheckExpireesAsync(CancellationToken ct = default)
        {
            // Récupérer toutes les prescriptions validées
            var prescriptions = await _unitOfWork.Prescriptions.GetExpireesAsync(ct);

            foreach (var prescription in prescriptions)
            {
                if (prescription.IsExpired())
                {
                    prescription.MarquerExpiree();
                }
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
