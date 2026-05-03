using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Services
{
    public class AssuranceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AssuranceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AssuranceComplementaire>> Get(Guid patientId) => await _unitOfWork.AssuranceComplementaires.GetActiveByPatient(patientId);

        public async Task Add(AssuranceComplementaire assurance)
        {
            await _unitOfWork.AssuranceComplementaires.AddAsync(assurance);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(AssuranceComplementaire assurance)
        {
            _unitOfWork.AssuranceComplementaires.Update(assurance);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
