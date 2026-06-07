using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Statistique_microservice.Infrastructure.Extensions;
using Statistique_microservice.Infrastructure.Persistence;
using Statistique_microservice.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ════════════════════════════════════════════════
// SERILOG
// ════════════════════════════════════════════════
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ════════════════════════════════════════════════
// SERVICES
// ════════════════════════════════════════════════
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var redisCs = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
var rabbitCs = $"amqp://{builder.Configuration["RabbitMq:Username"]}:" +
               $"{builder.Configuration["RabbitMq:Password"]}@" +
               $"{builder.Configuration["RabbitMq:Host"]}/";

builder.Services
    .AddDatabase(connStr)
    .AddRepositories()
    .AddApplicationServices()
    .AddCalculateurs()
    .AddRedisCache(builder.Configuration)
    .AddMessaging(builder.Configuration)
    .AddHangfireJobs(connStr)
    .AddJwtFromAuthSvc(builder.Configuration)
    .AddValidatorsAndMapping();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ──────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Statistique.svc",
        Version = "v1",
        Description = "Microservice de statistiques et KPIs pour DigiHealth"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
});

// ── Health checks ─────────────────────────────────────────────────────
// ✅ FIX DÉFINITIF : on n'utilise PAS AddSqlServer() du package HealthChecks
// car il entre en conflit avec AddSqlServer<TContext> d'EF Core.
// On enregistre un check SQL manuel avec AddCheck<T> — zéro ambiguïté.
builder.Services
    .AddHealthChecks()
    .AddCheck("sqlserver", new SqlServerHealthCheck(connStr))  // ← check manuel
    .AddRedis(redisCs)
    .AddRabbitMQ(rabbitConnectionString: rabbitCs);

// ════════════════════════════════════════════════
// BUILD
// ════════════════════════════════════════════════
var app = builder.Build();

// ── Migrations automatiques ───────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StatistiqueDbContext>();
    await db.Database.MigrateAsync();
}

// ════════════════════════════════════════════════
// PIPELINE MIDDLEWARES
// ════════════════════════════════════════════════
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Statistique.svc v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter()]
});

HangfireJobRegistrar.EnregistrerJobs();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy]   = StatusCodes.Status200OK,
            [HealthStatus.Degraded]  = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });
app.MapHealthChecks("/health/live",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = _ => false
    });

app.MapControllers();
app.Run();

// ════════════════════════════════════════════════
// Check SQL Server manuel — pas de conflit de namespace
// ════════════════════════════════════════════════
public sealed class SqlServerHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public SqlServerHealthCheck(string connectionString)
        => _connectionString = connectionString;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            await cmd.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy("SQL Server opérationnel.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("SQL Server inaccessible.", ex);
        }
    }
}