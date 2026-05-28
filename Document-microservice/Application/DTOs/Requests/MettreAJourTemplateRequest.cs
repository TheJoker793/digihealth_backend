namespace Document_microservice.Application.DTOs.Requests
{
    public record MettreAJourTemplateRequest(
    string ContenuHtml,
    string[] Variables,
    string NouvelleVersion
);
}
