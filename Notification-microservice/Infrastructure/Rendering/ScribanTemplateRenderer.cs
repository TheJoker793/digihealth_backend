using Notification_microservice.Domain.Interfaces;
using Scriban;




namespace Notification_microservice.Infrastructure.Rendering
{
    public class ScribanTemplateRenderer : ITemplateRenderer
    {
        private readonly ILogger<ScribanTemplateRenderer> _logger;
        public ScribanTemplateRenderer(ILogger<ScribanTemplateRenderer> logger)
        {
            _logger = logger;
        }

        public async Task<string> RendreAsync(
        string templateTexte,
        Dictionary<string, string> variables,
        CancellationToken ct = default)
        {
            try
            {
                var template = Template.Parse(templateTexte);

                if (template.HasErrors)
                {
                    var erreurs = string.Join(", ", template.Messages.Select(m => m.Message));
                    _logger.LogError("Erreur parsing template Scriban : {Erreurs}", erreurs);
                    throw new InvalidOperationException($"Template invalide : {erreurs}");
                }

                // Convertir Dictionary → objet Scriban
                var scriptObject = new Scriban.Runtime.ScriptObject();
                foreach (var (cle, valeur) in variables)
                    scriptObject.Add(cle, valeur);

                var contexte = new TemplateContext();
                contexte.PushGlobal(scriptObject);

                var rendu = await template.RenderAsync(contexte);
                return rendu;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Erreur rendu Scriban");
                throw new InvalidOperationException($"Erreur rendu template : {ex.Message}", ex);
            }
        }
    }
}
