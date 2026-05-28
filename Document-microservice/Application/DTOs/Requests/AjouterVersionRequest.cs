namespace Document_microservice.Application.DTOs.Requests
{
    public record AjouterVersionRequest(
    Guid DocumentId,
    IFormFile Fichier,
    string? CommentaireVersion = null
);
}
