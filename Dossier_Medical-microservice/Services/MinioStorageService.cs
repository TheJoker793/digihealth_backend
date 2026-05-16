using Dossier_Medical_microservice.Domain.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace Dossier_Medical_microservice.Services
{
    public class MinioStorageService : IStorageService
    {
        private readonly MinioClient _client;
        private readonly string _bucketName = "documents-medical";

        public MinioStorageService(MinioClient client)
        {
            _client = client;
        }

        // Upload avec nom explicite
        public async Task UploadAsync(string objectName, Stream data, string contentType, CancellationToken ct = default)
        {
            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithContentType(contentType)
                .WithObjectSize(data.Length), ct);
        }

        // Upload simplifié (retourne chemin généré)
        public async Task<string> UploadAsync(Stream stream, string fileName, string mimeType, CancellationToken ct = default)
        {
            var objectName = $"{Guid.NewGuid()}-{fileName}";
            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithContentType(mimeType)
                .WithObjectSize(stream.Length), ct);

            return objectName;
        }

        // Download
        public async Task<Stream> DownloadAsync(string objectName, CancellationToken ct = default)
        {
            var ms = new MemoryStream();
            await _client.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream => stream.CopyTo(ms)), ct);

            ms.Position = 0;
            return ms;
        }

        // Delete
        public async Task DeleteAsync(string cheminFichier, CancellationToken ct = default)
        {
            await _client.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(cheminFichier), ct);
        }

        // Presigned URL
        public async Task<string> GetPresignedUrlAsync(string cheminFichier, int expiresMinutes = 15, CancellationToken ct = default)
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(cheminFichier)
                .WithExpiry(expiresMinutes * 60); // en secondes

            return await _client.PresignedGetObjectAsync(args);
        }

    }
}
