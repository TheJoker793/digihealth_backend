using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Domain.Enums;
using Document_microservice.Domain.Interfaces;

namespace Document_microservice.Application.Services
{
    public class GenerationPdfService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPdfGeneratorService _pdfGenerator;
        private readonly IStorageService _storage;
        private readonly INumeroDocumentGenerator _numeroGen;
        private readonly IEventPublisher _publisher;

        public GenerationPdfService(
            IUnitOfWork uow,
            IPdfGeneratorService pdfGenerator,
            IStorageService storage,
            INumeroDocumentGenerator numeroGen,
            IEventPublisher publisher)
        {
            _uow = uow;
            _pdfGenerator = pdfGenerator;
            _storage = storage;
            _numeroGen = numeroGen;
            _publisher = publisher;
        }

        /// <summary>
        /// Génère un PDF depuis un template, l'upload dans MinIO et crée le DocumentMedical.
        /// Utilisé par : génération automatique d'ordonnance, compte-rendu…
        /// </summary>
        public async Task<DocumentResponse> GenererEtSauvegarderAsync(
            GenererPdfRequest request,
            CancellationToken ct = default)
        {
            // 1. Charger le template
            var template = await _uow.Templates.GetByIdAsync(request.TemplateId, ct)
                ?? throw new KeyNotFoundException($"Template {request.TemplateId} introuvable.");

            // 2. Rendre le contenu HTML avec les variables
            var contenuRendu = template.Rendre(request.Variables);

            // 3. Générer le PDF (QuestPDF)
            var pdfBytes = await _pdfGenerator.GenererDepuisHtmlAsync(contenuRendu, ct);

            // 4. Numéro de document
            var numero = await _numeroGen.GenererAsync(request.CabinetId, ct);

            // 5. Créer l'agrégat
            var titre = request.Variables.GetValueOrDefault("titre", template.Nom);
            var document = Domain.Entities.DocumentMedical.Create(
                numero, titre, template.TypeDocument,
                request.PatientId, request.MedecinId, request.CabinetId,
                request.ConsultationId);

            // 6. Uploader le PDF
            var bucket = $"cabinet-{request.CabinetId}";
            var chemin = $"patients/{request.PatientId}/{numero}/v1.pdf";
            using var stream = new MemoryStream(pdfBytes);
            await _storage.UploadAsync(stream, chemin, "application/pdf", bucket, ct);

            // 7. Ajouter version 1
            var metadonnees = Domain.ValueObjects.MetadonneeFichier.Create(
                auteur: $"{request.MedecinId}",
                nbPages: null);

            var checksum = Convert.ToHexString(
                System.Security.Cryptography.SHA256.HashData(pdfBytes)).ToLowerInvariant();

            document.AjouterVersion(chemin, $"{numero}.pdf",
                FormatFichier.PDF, pdfBytes.Length, checksum, metadonnees);

            document.Publier();

            // 8. Persister
            await _uow.Documents.AddAsync(document, ct);
            await _uow.SaveChangesAsync(ct);

            foreach (var evt in document.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            document.ClearDomainEvents();

            return ToResponse(document);
        }

        private static DocumentResponse ToResponse(Domain.Entities.DocumentMedical d)
        {
            var v = d.VersionActive;
            return new DocumentResponse(
                d.Id, d.Numero, d.Titre, d.Description,
                d.TypeDocument, d.Statut, d.EstArchive,
                d.PatientId, d.MedecinId, d.CabinetId, d.ConsultationId,
                d.CreatedAt, d.UpdatedAt,
                v is null ? null : new VersionDocumentResponse(
                    v.Id, v.NumeroVersion, v.NomFichier,
                    v.Format, v.TailleOctets, v.EstActive,
                    v.Metadonnees.Auteur, v.Metadonnees.NbPages, v.CreatedAt));
        }
    }
}
