using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
namespace Rendez_vous_microservice.Exceptions
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration configuration, HttpClient httpClient)
        {
            _next = next;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Missing JWT token" }));
                return;
            }

            try
            {
                // Récupération des clés JWKS depuis Auth.svc
                var jwksUrl = _configuration["Auth:JWKSUrl"]; // ex: http://auth-svc/.well-known/jwks.json
                var jwksJson = await _httpClient.GetStringAsync(jwksUrl);
                var jwks = new JsonWebKeySet(jwksJson);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Auth:Issuer"],
                    ValidAudience = _configuration["Auth:Audience"],
                    IssuerSigningKeys = jwks.Keys
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid JWT", details = ex.Message }));
            }
        }
    }

    // Extension pour ajouter le middleware
    public static class JwtValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtValidationMiddleware>();
        }
    }
}
