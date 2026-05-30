using System.Net;
using System.Text.Json;
using Document_microservice.Domain.Exceptions;

namespace Document_microservice.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                // ===== 404 NOT FOUND =====
                DocumentNotFoundException
                    or VersionIntrouvableException
                    => HttpStatusCode.NotFound,

                // ===== 409 CONFLICT =====
                DocumentArchiveException
                    => HttpStatusCode.Conflict,

                // ===== 400 BAD REQUEST =====
                FormatNonAutoriseException
                    or FichierTropGrandException
                    or VirusDetecteException
                    => HttpStatusCode.BadRequest,

                // ===== 410 GONE (optionnel mais logique pour token expiré) =====
                PartageExpireException
                    => HttpStatusCode.Gone,

                // ===== DEFAULT =====
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                status = (int)statusCode,
                error = ex.GetType().Name,
                message = ex.Message,
                traceId = context.TraceIdentifier
            };

            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}