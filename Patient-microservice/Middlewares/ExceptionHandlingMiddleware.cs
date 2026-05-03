using Microsoft.AspNetCore.Http;
using Patient_microservice.Exceptions;
using System.Net;
using System.Text.Json;

namespace Patient_microservice.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex) // Exceptions métier
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,       // 404
                    ConflictException => (int)HttpStatusCode.Conflict,       // 409
                    ValidationException => (int)HttpStatusCode.BadRequest,   // 400
                    DoublonException => (int)HttpStatusCode.Conflict,        // 409
                    _ => (int)HttpStatusCode.BadRequest
                };

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex) // Exceptions système
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

                var result = JsonSerializer.Serialize(new { error = "Une erreur interne est survenue.", details = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }

    // Extension pour simplifier l'ajout du middleware
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
