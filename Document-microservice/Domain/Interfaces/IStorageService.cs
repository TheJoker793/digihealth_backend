namespace Document_microservice.Domain.Interfaces
{
    public interface IStorageService
    {
        /// <summary>Upload un fichier et retourne le chemin de stockage.</summary>
        Task<string> UploadAsync(
            Stream contenu,
            string nomFichier,
            string contentType,
            string bucket,
            CancellationToken ct = default);

        /// <summary>Télécharge un fichier depuis le stockage.</summary>
        Task<Stream> DownloadAsync(string chemin, CancellationToken ct = default);

        /// <summary>Supprime un fichier du stockage.</summary>
        Task SupprimerAsync(string chemin, CancellationToken ct = default);

        /// <summary>Génère une URL pré-signée valable <paramref name="duree"/>.</summary>
        Task<string> GenererUrlPresigneeAsync(
            string chemin,
            TimeSpan duree,
            CancellationToken ct = default);

        Task<bool> ExisteAsync(string chemin, CancellationToken ct = default);
    }
}
