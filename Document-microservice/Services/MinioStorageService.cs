using Document_microservice.Domain.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace Document_microservice.Services
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _minio;
        private readonly ILogger<MinioStorageService> _logger;
        public MinioStorageService(IMinioClient minio, ILogger<MinioStorageService> logger)
        {
            _minio = minio;
            _logger = logger;
        }
        // ── Upload ───────────────────────────────────────────────
        public async Task<string> UploadAsync(
            Stream contenu,
            string nomFichier,
            string contentType,
            string bucket,
            CancellationToken ct = default)
        {
            await AssurerBucketExisteAsync(bucket, ct);

            var args = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(nomFichier)
                .WithStreamData(contenu)
                .WithObjectSize(contenu.Length)
                .WithContentType(contentType);

            await _minio.PutObjectAsync(args, ct);

            _logger.LogInformation("Fichier uploadé : {Bucket}/{Chemin}", bucket, nomFichier);
            return $"{bucket}/{nomFichier}";
        }

        // ── Download ─────────────────────────────────────────────
        public async Task<Stream> DownloadAsync(string chemin, CancellationToken ct = default)
        {
            var (bucket, objet) = SplitChemin(chemin);
            var ms = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objet)
                .WithCallbackStream(stream => stream.CopyTo(ms));

            await _minio.GetObjectAsync(args, ct);
            ms.Position = 0;
            return ms;
        }

        // ── Supprimer ────────────────────────────────────────────
        public async Task SupprimerAsync(string chemin, CancellationToken ct = default)
        {
            var (bucket, objet) = SplitChemin(chemin);

            var args = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objet);

            await _minio.RemoveObjectAsync(args, ct);
            _logger.LogInformation("Fichier supprimé : {Chemin}", chemin);
        }
        // ── URL pré-signée ───────────────────────────────────────
        public async Task<string> GenererUrlPresigneeAsync(
            string chemin,
            TimeSpan duree,
            CancellationToken ct = default)
        {
            var (bucket, objet) = SplitChemin(chemin);

            var args = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objet)
                .WithExpiry((int)duree.TotalSeconds);

            return await _minio.PresignedGetObjectAsync(args);
        }

        // ── Existence ────────────────────────────────────────────
        public async Task<bool> ExisteAsync(string chemin, CancellationToken ct = default)
        {
            try
            {
                var (bucket, objet) = SplitChemin(chemin);
                var args = new StatObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objet);

                await _minio.StatObjectAsync(args, ct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ── Helpers ──────────────────────────────────────────────
        private async Task AssurerBucketExisteAsync(string bucket, CancellationToken ct)
        {
            var existsArgs = new BucketExistsArgs().WithBucket(bucket);
            var existe = await _minio.BucketExistsAsync(existsArgs, ct);

            if (!existe)
            {
                var makeArgs = new MakeBucketArgs().WithBucket(bucket);
                await _minio.MakeBucketAsync(makeArgs, ct);
                _logger.LogInformation("Bucket MinIO créé : {Bucket}", bucket);
            }
        }

        private static (string bucket, string objet) SplitChemin(string chemin)
        {
            var idx = chemin.IndexOf('/');
            if (idx < 0)
                throw new ArgumentException($"Chemin MinIO invalide : {chemin}");

            return (chemin[..idx], chemin[(idx + 1)..]);
        }
    }
}
