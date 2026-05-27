namespace Facturation_microservice.Domain.Interfaces
{
    public interface ICnamService
    {
        Task<decimal> GetTauxRemboursementAsync(string codeActe);

        Task<bool> ValiderAffiliationAsync(string numeroAffilie);
    }
}
