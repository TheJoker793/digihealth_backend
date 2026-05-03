using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Exceptions;

namespace Auth_microservice.Services
{
    public class PasswordResetService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;
        private readonly IEmailService _email;

        public PasswordResetService(
        IUnitOfWork uow,
        IPasswordHasher hasher,
        IEmailService email)
        {
            _uow = uow;
            _hasher = hasher;
            _email = email;
        }

        // =========================
        // SEND RESET LINK
        // =========================
        public async Task SendResetLinkAsync(string email)
        {
            var user = await _uow.Users.GetByLoginAsync(email)
                ?? throw new NotFoundException("User not found");

            var token = Guid.NewGuid().ToString();

            // normalement stocké en DB / cache
            await _email.SendResetPasswordAsync(email, token);
        }

        // =========================
        // RESET PASSWORD
        // =========================
        public async Task ResetPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            user.ChangePassword(_hasher.Hash(newPassword));

            await _uow.SaveChangesAsync();
        }
    }
}
