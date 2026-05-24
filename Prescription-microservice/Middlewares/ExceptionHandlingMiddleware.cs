using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Prescription_microservice.Application.Exceptions;

namespace Prescription_microservice.Infrastructure.Middlewares
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
                InteractionBloquanteException => (int)HttpStatusCode.UnprocessableEntity, // 422
                NotFoundException => (int)HttpStatusCode.NotFound,                       // 404
                ConflictException => (int)HttpStatusCode.Conflict,                       // 409
                ValidationException => (int)HttpStatusCode.BadRequest,                   // 400
                _ => (int)HttpStatusCode.InternalServerError                            // 500
            };

            var problem = new
            {
                status = statusCode,
                title = ex.GetType().Name,
                detail = ex.Message,
                traceId = context.TraceIdentifier
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }

    // Extension pour Program.cs
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
