using Auth_microservice.Domain.Interfaces;

namespace Auth_microservice.Middlewares
{
    public class BlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public BlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenBlacklist blacklist)
        {
            var jti = context.User?.FindFirst("jti")?.Value;

            if (!string.IsNullOrEmpty(jti))
            {
                if (await blacklist.IsBlacklistedAsync(jti))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token revoked");
                    return;
                }
            }

            await _next(context);
        }
    }
}