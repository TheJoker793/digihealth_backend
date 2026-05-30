using static Notification_microservice.Domain.Exceptions.NotificationExceptions;
using System.Net;
using System.Text.Json;

namespace Notification_microservice.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception non gérée : {Message}", ex.Message);
                await EcrireReponseErreurAsync(context, ex);
            }
        }

        private static async Task EcrireReponseErreurAsync(
            HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var (statusCode, titre, detail) = exception switch
            {
                // ── 404 Not Found ────────────────────────────────
                NotificationIntrouvableException e =>
                    (HttpStatusCode.NotFound, "Notification introuvable", e.Message),

                KeyNotFoundException e =>
                    (HttpStatusCode.NotFound, "Ressource introuvable", e.Message),

                // ── 400 Bad Request ──────────────────────────────
                TemplateIntrouvableException e =>
                    (HttpStatusCode.BadRequest, "Template introuvable", e.Message),

                TemplateInactifException e =>
                    (HttpStatusCode.BadRequest, "Template inactif", e.Message),

                ArgumentException e =>
                    (HttpStatusCode.BadRequest, "Paramètre invalide", e.Message),

                InvalidOperationException e =>
                    (HttpStatusCode.BadRequest, "Opération invalide", e.Message),

                // ── 403 Forbidden ────────────────────────────────
                DestinataireOptOutException e =>
                    (HttpStatusCode.Forbidden, "Destinataire opt-out", e.Message),

                CanalNonAutoriseException e =>
                    (HttpStatusCode.Forbidden, "Canal non autorisé", e.Message),

                HorsPlageHoraireException e =>
                    (HttpStatusCode.Forbidden, "Hors plage horaire", e.Message),

                // ── 409 Conflict ─────────────────────────────────
                MaxTentativesAtteinteException e =>
                    (HttpStatusCode.Conflict, "Max tentatives atteint", e.Message),

                NotificationAnnuleeException e =>
                    (HttpStatusCode.Conflict, "Notification annulée", e.Message),

                // ── 500 Internal ─────────────────────────────────
                _ => (HttpStatusCode.InternalServerError,
                      "Erreur interne du serveur",
                      "Une erreur inattendue est survenue.")
            };

            context.Response.StatusCode = (int)statusCode;

            var problem = new
            {
                type = $"https://httpstatuses.com/{(int)statusCode}",
                title = titre,
                status = (int)statusCode,
                detail = detail,
                instance = context.Request.Path.Value,
                traceId = context.TraceIdentifier,
            };

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
