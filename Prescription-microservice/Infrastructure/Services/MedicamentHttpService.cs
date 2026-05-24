using Prescription_microservice.Domain.Interfaces;

namespace Prescription_microservice.Infrastructure.Services
{
    public class MedicamentHttpService : IMedicamentService
    {
        private readonly HttpClient _httpClient;

        public MedicamentHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        // Vérifie les interactions entre médicaments
        public async Task<IEnumerable<(Guid MedicamentA, Guid MedicamentB, string Severite)>> CheckInteractionsAsync(IEnumerable<Guid> medicamentIds)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/medicaments/check-interactions", medicamentIds);
            response.EnsureSuccessStatusCode();

            var interactions = await response.Content.ReadFromJsonAsync<IEnumerable<(Guid MedicamentA, Guid MedicamentB, string Severite)>>();
            return interactions ?? Enumerable.Empty<(Guid, Guid, string)>();
        }

        // Vérifie si un médicament existe
        public async Task<bool> ExistsAsync(Guid medicamentId)
        {
            var response = await _httpClient.GetAsync($"/api/medicaments/{medicamentId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        // Vérifie une liste d’IDs et retourne ceux existants
        public async Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> medicamentIds)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/medicaments/check-existence", medicamentIds);
            response.EnsureSuccessStatusCode();

            var existingIds = await response.Content.ReadFromJsonAsync<IEnumerable<Guid>>();
            return existingIds ?? Enumerable.Empty<Guid>();
        }
    }
}
