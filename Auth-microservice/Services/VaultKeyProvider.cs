using Auth_microservice.Domain.Interfaces;

namespace Auth_microservice.Services
{
    public class VaultKeyProvider : IKeyProvider
    {
        public string GetPrivateKey()
        {
            // Azure Key Vault / env secret
            return File.ReadAllText("private.pem");
        }

        public string GetPublicKey()
        {
            return File.ReadAllText("public.pem");
        }
    }
}
