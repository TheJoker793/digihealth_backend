namespace Dossier_Medical_microservice.Application.DTOs.Requests
{
    public class UploadDocumentForm
    {
        public IFormFile Fichier { get; set; } = default!;
        public string Titre { get; set; } = string.Empty;
        public int TypeDocument { get; set; }
    }
}
