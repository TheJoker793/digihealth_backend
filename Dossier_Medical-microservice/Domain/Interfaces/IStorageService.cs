namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream stream, string fileName, string mimeType, CancellationToken ct = default);
        Task<Stream> DownloadAsync(string cheminFichier, CancellationToken ct = default);
        Task DeleteAsync(string cheminFichier, CancellationToken ct = default);
        Task<string> GetPresignedUrlAsync(string cheminFichier, int expiresMinutes = 15, CancellationToken ct = default);
    }
}
