using Auth_microservice.Domain.Interfaces;
using javax.crypto;
using OtpNet;

namespace Auth_microservice.Services
{
    public class OtpNetTotpService : ITotpService
    {
        public string GenerateSecret()
        {
            return Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
        }

        public string GenerateQrCodeUri(string secret, string login)
        {
            return $"otpauth://totp/DMI:{login}?secret={secret}&issuer=DMI";
        }

        public bool ValidateCode(string secret, string code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secret));
            return totp.VerifyTotp(code, out _);
        }
    }
}
