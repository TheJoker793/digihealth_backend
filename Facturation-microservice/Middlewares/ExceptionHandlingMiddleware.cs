using System.Net;
using static Facturation_microservice.Application.Exceptions.AppExceptions;

namespace Facturation_microservice.Middlewares
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                FactureDejaPayeeException => 422,
                MontantDepasseException => 400,
                FactureAnnuleeException => 409,
                _ => (int)HttpStatusCode.InternalServerError
            };

            return context.Response.WriteAsync(new
            {
                error = ex.Message
            }.ToString());
        }
    }
}