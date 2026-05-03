using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Services
{
    public class CouvertureSocialService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CouvertureSocialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CouvertureSociale>> Get(Guid patientId) => await _unitOfWork.CouvertureSociales.GetActiveByPatient(patientId);
        public async Task Add(CouvertureSociale couverture)
        {
            await _unitOfWork.CouvertureSociales.AddAsync(couverture);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(CouvertureSociale couverture)
        {
            _unitOfWork.CouvertureSociales.Update(couverture);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
