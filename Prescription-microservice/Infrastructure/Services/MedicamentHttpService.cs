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
        // Note: L'API Médicaments FR (data.gouv.fr) utilise des codes CIS (int).
        // Cette implémentation suppose que medicamentId correspond au code CIS.

        public async Task<bool> ExistsAsync(Guid medicamentId)
        {
            // Le code CIS est un identifiant numérique à 8 chiffres.
            // On extrait la partie numérique du Guid pour la démonstration.
            var cis = medicamentId.ToString().Split('-')[0];

            // Endpoint officiel : /v1/medicaments/{cis}
            var response = await _httpClient.GetAsync($"/v1/medicaments/{cis}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> medicamentIds)
        {
            var results = new List<Guid>();
            foreach (var id in medicamentIds)
            {
                if (await ExistsAsync(id)) results.Add(id);
            }
            return results;
        }

        public async Task<IEnumerable<(Guid MedicamentA, Guid MedicamentB, string Severite)>> CheckInteractionsAsync(IEnumerable<Guid> medicamentIds)
        {
            // L'API publique ne fournit pas de endpoint direct pour les interactions.
            // Cette logique devra être implémentée soit via un service tiers spécialisé (ex: Thériaque, Vidal),
            // soit en analysant les compositions croisées si les données sont disponibles.
            // Pour l'instant, on retourne une liste vide ou on conserve l'appel au microservice interne si disponible.
            return Enumerable.Empty<(Guid, Guid, string)>();
        }
    }
}
