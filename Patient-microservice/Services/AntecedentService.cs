using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Services
{
    public class AntecedentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AntecedentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Antecedent>> GetByPatient(Guid patientId) => await _unitOfWork.Antecedents.GetAntecedentsByPatientAsync(patientId);

        public async Task Add(Antecedent antecedent)
        {
            await _unitOfWork.Antecedents.AddAsync(antecedent);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(Antecedent antecedent)
        {
            _unitOfWork.Antecedents.Update(antecedent);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(Antecedent antecedent)
        {

            _unitOfWork.Antecedents.Remove(antecedent);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
