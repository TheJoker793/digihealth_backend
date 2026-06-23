using Dossier_Medical_microservice.Domain.Interfaces;
using System.Text.Json;

namespace Dossier_Medical_microservice.Services
{
    public class Cim11HttpService: ICim11Service
    {
        private readonly HttpClient _http;

        public Cim11HttpService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Cim11Result?> SearchAsync(string query, string lang = "en", CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/api/v1/cim11/recherche?q={query}&lang={lang}", ct);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<Cim11Result>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Cim11Detail?> GetByCodeAsync(string code, CancellationToken ct = default)
        {
            var response = await _http.GetAsync($"/api/v1/cim11/{code}", ct);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<Cim11Detail>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
