namespace Auth_microservice.Domain.Interfaces
{
    public interface IKeyProvider
    {
        string GetPrivateKey();

        string GetPublicKey();
    }
}
