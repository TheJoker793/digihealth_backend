using Auth_microservice.Exceptions;
using System.Net;

namespace Auth_microservice.Middlewares
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
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode code = ex switch
            {
                UnauthorizedException => HttpStatusCode.Unauthorized,
                NotFoundException => HttpStatusCode.NotFound,
                ConflictException => HttpStatusCode.Conflict,
                AccountLockedException => (HttpStatusCode)429,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message,
                status = (int)code
            });
        }
    }
}