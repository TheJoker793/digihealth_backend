using FluentValidation;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Prescription_microservice.Application.Services;
using Prescription_microservice.Application.Validators;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Jobs;
using Prescription_microservice.Infrastructure.Persistence;
using Prescription_microservice.Infrastructure.Persistence.Repositories;
using Prescription_microservice.Infrastructure.Services;
using StackExchange.Redis;

namespace Prescription_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // =========================
        // DATABASE
        // =========================
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<PrescriptionDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sql => sql.MigrationsAssembly(
                        typeof(PrescriptionDbContext).Assembly.FullName)));

            return services;
        }

        // =========================
        // REPOSITORIES + UNIT OF WORK
        // =========================
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<ILignePrescriptionRepository, LignePrescriptionRepository>();
            services.AddScoped<IInteractionRepository, InteractionRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        // =========================
        // APPLICATION SERVICES
        // =========================
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<PrescriptionService>();
            services.AddScoped<InteractionService>();
            services.AddScoped<ExpirationService>();

            return services;
        }

        // =========================
        // INFRASTRUCTURE SERVICES
        // =========================
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Redis
            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var redisConnection =
                    config.GetConnectionString("Redis")
                    ?? "localhost:6379";

                return ConnectionMultiplexer.Connect(redisConnection);
            });

            services.AddScoped<IPrescriptionCacheService, RedisPrescriptionCacheService>();
            services.AddScoped<INumeroPrescriptionGenerator, NumeroPrescriptionGenerator>();
            services.AddScoped<IPdfService, QuestPdfService>();
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            // HttpClient Médicament
            services.AddHttpClient<IMedicamentService, MedicamentHttpService>(client =>
            {
                client.BaseAddress = new Uri(
                    config["Services:MedicamentUrl"]
                    ?? "http://medicament-svc:5009");

                client.Timeout = TimeSpan.FromSeconds(10);

                client.DefaultRequestHeaders.Add(
                    "Accept",
                    "application/json");
            });

            return services;
        }

        // =========================
        // MASSTRANSIT + RABBITMQ
        // =========================
        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        config["RabbitMq:Host"] ?? "localhost",
                        h =>
                        {
                            h.Username(
                                config["RabbitMq:Username"] ?? "guest");

                            h.Password(
                                config["RabbitMq:Password"] ?? "guest");
                        });

                    cfg.UseMessageRetry(r =>
                        r.Intervals(
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(15),
                            TimeSpan.FromSeconds(30)));

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        // =========================
        // JWT AUTHENTICATION
        // =========================
        public static IServiceCollection AddJwtFromAuthSvc(
            this IServiceCollection services,
            IConfiguration config)
        {
            var jwksUri =
                config["Jwt:JwksUri"]
                ?? "http://auth-svc:5001/.well-known/jwks.json";

            var issuer =
                config["Jwt:Issuer"]
                ?? "DMI.Auth";

            var audience =
                config["Jwt:Audience"]
                ?? "DMI.Client";

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = issuer,
                            ValidAudience = audience,

                            ClockSkew = TimeSpan.Zero
                        };

                    options.ConfigurationManager =
                        new ConfigurationManager<OpenIdConnectConfiguration>(
                            jwksUri,
                            new OpenIdConnectConfigurationRetriever(),
                            new HttpDocumentRetriever
                            {
                                RequireHttps = false
                            });

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                System.Text.Json.JsonSerializer.Serialize(new
                                {
                                    error = "Token JWT manquant ou invalide.",
                                    status = 401
                                }));
                        },

                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                System.Text.Json.JsonSerializer.Serialize(new
                                {
                                    error = "Accès refusé — rôle insuffisant.",
                                    status = 403
                                }));
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MedecinOnly", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireRole("Medecin", "Admin"));

                options.AddPolicy("StaffOnly", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireRole(
                              "Medecin",
                              "Secretaire",
                              "Infirmier",
                              "Admin"));
            });

            return services;
        }

        // =========================
        // HANGFIRE
        // =========================
        public static IServiceCollection AddHangfireJobs(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddHangfire(config =>
                config.UseSqlServerStorage(connectionString));

            services.AddHangfireServer();

            return services;
        }

        // =========================
        // FLUENT VALIDATION
        // =========================
        public static IServiceCollection AddValidators(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreerPrescriptionValidator>();

            return services;
        }
    }
}