using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Exceptions;

namespace Auth_microservice.Services
{
    public class TwoFactorService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITotpService _totp;

        public TwoFactorService(IUnitOfWork uow, ITotpService totp)
        {
            _uow = uow;
            _totp = totp;
        }

        // =========================
        // SETUP 2FA
        // =========================
        public async Task<string> Setup2FAAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            var secret = _totp.GenerateSecret();

            user.Enable2FA(secret);

            await _uow.SaveChangesAsync();

            return _totp.GenerateQrCodeUri(secret, user.Login);
        }

        // =========================
        // CONFIRM SETUP
        // =========================
        public async Task<bool> ConfirmSetupAsync(Guid userId, string code)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            return _totp.ValidateCode(user.TotpSecret!, code);
        }

        // =========================
        // DISABLE 2FA
        // =========================
        public async Task Disable2FAAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            user.Disable2FA();

            await _uow.SaveChangesAsync();
        }

    }
}
