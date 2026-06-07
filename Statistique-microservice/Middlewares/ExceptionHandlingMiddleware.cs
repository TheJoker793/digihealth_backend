using Statistique_microservice.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Statistique_microservice.Middlewares
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
                await EcrireErreurAsync(context, ex);
            }
        }

        private static async Task EcrireErreurAsync(HttpContext ctx, Exception ex)
        {
            ctx.Response.ContentType = "application/problem+json";

            var (status, titre, detail) = ex switch
            {
                RapportIntrouvableException e =>
                    (HttpStatusCode.NotFound, "Rapport introuvable", e.Message),

                TableauDeBordIntrouvableException e =>
                    (HttpStatusCode.NotFound, "Tableau de bord introuvable", e.Message),

                AbonnementIntrouvableException e =>
                    (HttpStatusCode.NotFound, "Abonnement introuvable", e.Message),

                SnapshotIntrouvableException e =>
                    (HttpStatusCode.NotFound, "Snapshot introuvable", e.Message),

                KPIIntrouvableException e =>
                    (HttpStatusCode.NotFound, "KPI introuvable", e.Message),

                PeriodeInvalideException e =>
                    (HttpStatusCode.BadRequest, "Période invalide", e.Message),

                RapportAnnuleException e =>
                    (HttpStatusCode.Conflict, "Rapport annulé", e.Message),

                SnapshotDejaConsolideException e =>
                    (HttpStatusCode.Conflict, "Snapshot déjà consolidé", e.Message),

                ArgumentException e =>
                    (HttpStatusCode.BadRequest, "Paramètre invalide", e.Message),

                InvalidOperationException e =>
                    (HttpStatusCode.BadRequest, "Opération invalide", e.Message),

                _ => (HttpStatusCode.InternalServerError,
                      "Erreur interne", "Une erreur inattendue est survenue.")
            };

            ctx.Response.StatusCode = (int)status;

            var body = JsonSerializer.Serialize(new
            {
                type = $"https://httpstatuses.com/{(int)status}",
                title = titre,
                status = (int)status,
                detail = detail,
                instance = ctx.Request.Path.Value,
                traceId = ctx.TraceIdentifier,
            }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await ctx.Response.WriteAsync(body);
        }
    }
}
