using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;
using System.Text;
using System.Text.Json;

// =========================================================
// SERILOG – bootstrap logger (avant la construction du host)
// Capture les erreurs de démarrage
// =========================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Démarrage de DigiHealth.ApiGateway...");

    var builder = WebApplication.CreateBuilder(args);

    // =========================================================
    // SERILOG – logger complet depuis appsettings.json
    // =========================================================
    builder.Host.UseSerilog((context, services, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext());

    // =========================================================
    // CONFIGURATION
    // ocelot.json est chargé ICI (avant AddOcelot)
    // =========================================================
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                                                     optional: true, reloadOnChange: true)
        .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    // =========================================================
    // Injection dynamique des hosts depuis appsettings
    // En dev  → localhost
    // En prod → noms des services Docker (auth-microservice, …)
    // =========================================================
    var authHost = builder.Configuration["Hosts:Auth"] ?? "localhost";
    var patientHost = builder.Configuration["Hosts:Patient"] ?? "localhost";

    // Ocelot ne supporte pas les variables dans ocelot.json nativement.
    // On résout ${AUTH_HOST} / ${PATIENT_HOST} via un InMemoryCollection.
    var ocelotRaw = File.ReadAllText("ocelot.json")
        .Replace("${AUTH_HOST}", authHost)
        .Replace("${PATIENT_HOST}", patientHost);

    builder.Configuration.AddJsonStream(
        new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(ocelotRaw)));

    // =========================================================
    // CORS
    // =========================================================
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
    });

    // =========================================================
    // JWT AUTHENTICATION
    // Ocelot délègue la validation du token à ce middleware.
    // La clé DOIT être identique à celle du microservice Auth.
    // =========================================================
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtKey = jwtSection["Key"]
        ?? throw new InvalidOperationException("Jwt:Key manquant dans la configuration.");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            // Retourner du JSON propre au lieu des redirects HTML par défaut
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async ctx =>
                {
                    ctx.HandleResponse();
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    ctx.Response.ContentType = "application/json";
                    var body = JsonSerializer.Serialize(new
                    {
                        error = "Unauthorized",
                        message = "Un token JWT Bearer valide est requis."
                    });
                    await ctx.Response.WriteAsync(body);
                },
                OnForbidden = async ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    ctx.Response.ContentType = "application/json";
                    var body = JsonSerializer.Serialize(new
                    {
                        error = "Forbidden",
                        message = "Vous n'avez pas le rôle requis pour accéder à cette ressource."
                    });
                    await ctx.Response.WriteAsync(body);
                }
            };
        });

    builder.Services.AddAuthorization();

    // =========================================================
    // OCELOT + POLLY (circuit breaker / timeout)
    // =========================================================
    builder.Services
        .AddOcelot(builder.Configuration)
        .AddPolly();

    // =========================================================
    // HEALTH CHECKS
    // GET /health  →  200 OK si la gateway est vivante
    // =========================================================
    builder.Services
        .AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy("Gateway opérationnelle"));

    // =========================================================
    // BUILD
    // =========================================================
    var app = builder.Build();

    // Serilog request logging (remplace les logs de requête par défaut d'ASP.NET)
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} → {StatusCode} en {Elapsed:0.0}ms";
    });

    app.UseCors("AllowAll");

    // Health endpoint en dehors d'Ocelot, accessible sans JWT
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy]   = StatusCodes.Status200OK,
            [HealthStatus.Degraded]  = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

    app.UseAuthentication();
    app.UseAuthorization();

    // Ocelot DOIT être le dernier middleware
    await app.UseOcelot();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La gateway a planté au démarrage.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
