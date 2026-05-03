using Patient_microservice.Domain.Entities;

namespace Patient_microservice.Services
{
    public class PieceIdentiteService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PieceIdentiteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PieceIdentite>> GetByPatient(Guid patientId) => await _unitOfWork.PieceIdentites.GetByPatient(patientId);
        public async Task Add(PieceIdentite piece)
        {
            await _unitOfWork.PieceIdentites.AddAsync(piece);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(PieceIdentite piece)
        {
            _unitOfWork.PieceIdentites.Update(piece);
            await _unitOfWork.SaveChangesAsync();
        }

        
    }
}
