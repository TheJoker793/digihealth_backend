namespace Document_microservice.Domain.Interfaces
{
    public interface IPdfGeneratorService
    {
        /// <summary>Génère un PDF à partir d'un contenu HTML rendu.</summary>
        Task<byte[]> GenererDepuisHtmlAsync(
            string contenuHtml,
            CancellationToken ct = default);

        /// <summary>Génère un PDF à partir d'un template et de variables.</summary>
        Task<byte[]> GenererDepuisTemplateAsync(
            string nomTemplate,
            Dictionary<string, string> variables,
            CancellationToken ct = default);
    }
}
