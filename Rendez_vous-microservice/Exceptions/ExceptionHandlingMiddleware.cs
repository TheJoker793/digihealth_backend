
using Rendez_vous_microservice.Application.Validators;
using System.Net;
using System.Text.Json;
namespace Rendez_vous_microservice.Exceptions

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

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = ex.Message;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    break;
                case CreneauIndisponibleException:
                    statusCode = HttpStatusCode.Conflict; // 409
                    break;
                case ValidationException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    break;
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict; // 409
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                status = context.Response.StatusCode
            });

            await context.Response.WriteAsync(result);
        }
    }

    // Extension pour ajouter le middleware
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
