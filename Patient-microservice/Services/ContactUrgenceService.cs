using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Services
{
    public class ContactUrgenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactUrgenceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ContactUrgence>> GetByPatient(Guid patientId) => await _unitOfWork.ContactUrgences.GetByPatient(patientId);
        public async Task Add(ContactUrgence contact)
        {
            await _unitOfWork.ContactUrgences.AddAsync(contact);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(ContactUrgence contact)
        {
            _unitOfWork.ContactUrgences.Update(contact);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(ContactUrgence contact)
        {
             _unitOfWork.ContactUrgences.Remove(contact);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
