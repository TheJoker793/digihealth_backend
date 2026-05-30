using Document_microservice.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Document_microservice.Services
{
    public class DocumentCacheService : IDocumentCacheService
    {
        private readonly IMemoryCache _cache;

        private const string Prefix = "doc_cache_";

        public DocumentCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<string?> GetUrlPresigneeAsync(string chemin)
        {
            var key = BuildKey(chemin);

            _cache.TryGetValue(key, out string? url);

            return Task.FromResult(url);
        }

        public Task SetUrlPresigneeAsync(string chemin, string url, TimeSpan expiration)
        {
            var key = BuildKey(chemin);

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            _cache.Set(key, url, options);

            return Task.CompletedTask;
        }

        public Task InvaliderAsync(Guid documentId)
        {
            // Ici on suppose que le "chemin" contient l'id du document
            // ou qu'on stocke une clé basée dessus

            var keyPattern = $"{Prefix}{documentId}";

            // IMemoryCache ne supporte pas le "wildcard remove"
            // donc solution classique : on stocke aussi une clé directe

            _cache.Remove(keyPattern);

            return Task.CompletedTask;
        }

        private string BuildKey(string chemin)
        {
            return $"{Prefix}{chemin}";
        }
    }
}