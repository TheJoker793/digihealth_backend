using Facturation_microservice.Domain.Interfaces;

namespace Facturation_microservice.Infrastructure.Services
{
    public class CnamHttpService : ICnamService
    {
        private readonly HttpClient _httpClient;
        public CnamHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetTauxRemboursementAsync(string codeActe)
        {
            var response = await _httpClient.GetFromJsonAsync<decimal>(
                $"cnam/taux/{codeActe}");

            return response;
        }

        public async Task<bool> ValiderAffiliationAsync(string numeroAffilie)
        {
            var response = await _httpClient.GetFromJsonAsync<bool>(
                $"cnam/affiliation/{numeroAffilie}");

            return response;
        }

    }
}
