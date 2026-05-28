namespace Document_microservice.Domain.Interfaces
{
    public interface IDocumentCacheService
    {
        Task<string?> GetUrlPresigneeAsync(string chemin);
        Task SetUrlPresigneeAsync(string chemin, string url, TimeSpan expiration);
        Task InvaliderAsync(Guid documentId);
    }
}
