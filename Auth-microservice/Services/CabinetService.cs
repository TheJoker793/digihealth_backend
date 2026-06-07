using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.DTOs.Requests;
using Auth_microservice.DTOs.Responses;

namespace Auth_microservice.Services
{
    public class CabinetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CabinetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================
        // CREATE
        // =====================
        public async Task<CabinetDto> CreateAsync(CreateCabinetDto dto)
        {
            if (await _unitOfWork.Cabinets.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email déjà utilisé");

            if (await _unitOfWork.Cabinets.ExistsByTelephoneAsync(dto.Telephone))
                throw new Exception("Téléphone déjà utilisé");

            var cabinet = new Cabinet
            {
                Id = Guid.NewGuid(),
                Nom = dto.Nom,
                Email = dto.Email,
                Adresse = dto.Adresse,
                Telephone = dto.Telephone,
                Telephone2 = dto.Telephone2,
                Actif = dto.Actif,
            };

            await _unitOfWork.Cabinets.AddAsync(cabinet);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(cabinet);
        }

        // =====================
        // GET BY ID
        // =====================
        public async Task<CabinetDto?> GetByIdAsync(Guid id)
        {
            var cabinet = await _unitOfWork.Cabinets.GetByIdAsync(id);
            return cabinet == null ? null : MapToDto(cabinet);
        }

        // =====================
        // GET ALL
        // =====================
        public async Task<IEnumerable<CabinetDto>> GetAllAsync()
        {
            var cabinets = await _unitOfWork.Cabinets.GetAllAsync();
            return cabinets.Select(MapToDto);
        }

        // =====================
        // GET ACTIVE
        // =====================
        public async Task<IEnumerable<CabinetDto>> GetActiveAsync()
        {
            var cabinets = await _unitOfWork.Cabinets.GetActiveAsync();
            return cabinets.Select(MapToDto);
        }

        // =====================
        // UPDATE
        // =====================
        public async Task<CabinetDto> UpdateAsync(UpdateCabinetDto dto)
        {
            var cabinet = await _unitOfWork.Cabinets.GetByIdAsync(dto.Id);

            if (cabinet == null)
                throw new Exception("Cabinet introuvable");

            // check email unique (optionnel mais recommandé)
            if (cabinet.Email != dto.Email &&
                await _unitOfWork.Cabinets.ExistsByEmailAsync(dto.Email))
            {
                throw new Exception("Email déjà utilisé");
            }

            // check téléphone unique (optionnel)
            if (cabinet.Telephone != dto.Telephone &&
                await _unitOfWork.Cabinets.ExistsByTelephoneAsync(dto.Telephone))
            {
                throw new Exception("Téléphone déjà utilisé");
            }

            cabinet.Nom = dto.Nom;
            cabinet.Email = dto.Email;
            cabinet.Adresse = dto.Adresse;
            cabinet.Telephone = dto.Telephone;
            cabinet.Telephone2 = dto.Telephone2;
            cabinet.Actif = dto.Actif;

            await _unitOfWork.SaveChangesAsync();

            return MapToDto(cabinet);
        }

        // =====================
        // DELETE
        // =====================
        public async Task<bool> DeleteAsync(Guid id)
        {
            var cabinet = await _unitOfWork.Cabinets.GetByIdAsync(id);

            if (cabinet == null)
                return false;

            await _unitOfWork.Cabinets.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // =====================
        // MAPPING
        // =====================
        private CabinetDto MapToDto(Cabinet c)
        {
            return new CabinetDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Email = c.Email,
                Adresse = c.Adresse,
                Telephone = c.Telephone,
                Telephone2 = c.Telephone2,
                Actif = c.Actif,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            };
        }
    }
}