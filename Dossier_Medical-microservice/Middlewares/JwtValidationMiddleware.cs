using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace Dossier_Medical_microservice.Middlewares
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jwksUrl = _config["Auth:JWKS"];
                var jwksJson = await client.GetStringAsync(jwksUrl);

                var keys = new JsonWebKeySet(jwksJson);
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _config["Auth:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Auth:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKeys = keys.Keys
                }, out _);

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    title = "Unauthorized",
                    detail = ex.Message,
                    status = context.Response.StatusCode
                }));
            }
        }
    }

    public static class JwtValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtValidation(this IApplicationBuilder app)
            => app.UseMiddleware<JwtValidationMiddleware>();
    }
}
