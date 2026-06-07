using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Enums;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Exceptions;

namespace Auth_microservice.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;

        public UserService(IUnitOfWork uow, IPasswordHasher hasher)
        {
            _uow = uow;
            _hasher = hasher;
        }

        // =========================
        // CREATE USER
        // =========================

        public async Task <IEnumerable<User>> GetAllUsersAsync() 
        {
            var users = await _uow.Users.GetAllAsync();
            return users;
        }
        public async Task<Guid> CreateUserAsync(string login, string password, Role role, string cabinetId)
        {
            var existing = await _uow.Users.GetByLoginAsync(login);
            if (existing != null)
                throw new ConflictException("User already exists");

            var user = User.Create(
                login,
                _hasher.Hash(password),
                role,
                cabinetId
            );

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            return user.Id;
        }

        // =========================
        // DISABLE USER
        // =========================
        public async Task DisableUserAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            user.Deactivate();

            await _uow.SaveChangesAsync();
        }

        // =========================
        // CHANGE ROLE
        // =========================
        public async Task ChangeRoleAsync(Guid userId, Role newRole)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            ///user.ChangeRole(newRole);

            await _uow.SaveChangesAsync();
        }






    }
}
