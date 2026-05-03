namespace Auth_microservice.Domain.Interfaces
{
    public interface ITotpService
    {
        string GenerateSecret();

        string GenerateQrCodeUri(string secret, string login);

        bool ValidateCode(string secret, string code);
    }
}
