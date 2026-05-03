using Patient_microservice.Exceptions;

namespace Patient_microservice.Services
{
    public class DoublonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoublonService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Detect(string cin, string passeport, string nom, DateOnly dateNaissance)
        {
            var patient = await _unitOfWork.Patients.GetAllAsync();
            return patient.Any(p => p.DateNaissance == dateNaissance &&
                                    (p.PieceIdentites.Any(pi => pi.Numero == cin) ||
                                     p.PieceIdentites.Any(pi => pi.Numero == passeport)));
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
