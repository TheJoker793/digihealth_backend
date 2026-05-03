using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Enums;
using Patient_microservice.Exceptions;

namespace Patient_microservice.Services
{
    public class PatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Patient> Create(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return patient;
        }
        public async Task<Patient?> GetById(Guid id) => await _unitOfWork.Patients.GetByIdAsync(id);
        public async Task<IEnumerable<Patient>> Search(string nom, string prenom) => await _unitOfWork.Patients.GetAllAsync();
        public async Task Update(Patient patient)
        {
            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task ChangeStatut(Guid patientId, StatutPatient statut)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientId) ?? throw new NotFoundException("Patient introuvable");
            patient.StatutPatient = statut;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Merge(Guid sourceId, Guid targetId)
        {
            var source = await _unitOfWork.Patients.GetByIdAsync(sourceId);
            if (source == null) throw new NotFoundException("Source introuvable");
            source.MergeWith(targetId);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
