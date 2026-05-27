namespace Facturation_microservice.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(HeaderName))
            {
                context.Request.Headers[HeaderName] = Guid.NewGuid().ToString();
            }

            context.Response.Headers[HeaderName] =
                context.Request.Headers[HeaderName]!;

            await _next(context);
        }
    }
}