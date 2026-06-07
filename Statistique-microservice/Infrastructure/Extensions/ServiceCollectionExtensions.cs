using MassTransit;
using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Application.Services;
using Statistique_microservice.Application.Validators;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Infrastructure.Calcul;
using Statistique_microservice.Infrastructure.Persistence.Repositories;
using Statistique_microservice.Infrastructure.Persistence;
using Statistique_microservice.Application.Consumers;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using FluentValidation;
using Hangfire;

namespace Statistique_microservice.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StatistiqueDbContext>(opt =>
                opt.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(
                        typeof(StatistiqueDbContext).Assembly.FullName)));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRapportRepository, RapportRepository>();
            services.AddScoped<IKPIRepository, KPIRepository>();
            services.AddScoped<ISnapshotRepository, SnapshotRepository>();
            services.AddScoped<ITableauDeBordRepository, TableauDeBordRepository>();
            services.AddScoped<IAbonnementRepository, AbonnementRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<RapportService>();
            services.AddScoped<KPIService>();
            services.AddScoped<SnapshotService>();
            services.AddScoped<TableauDeBordService>();
            services.AddScoped<AbonnementService>();
            return services;
        }

        public static IServiceCollection AddCalculateurs(this IServiceCollection services)
        {
            services.AddScoped<ICalculateurKPI, CalculateurKPIService>();
            return services;
        }

        public static IServiceCollection AddRedisCache(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { config.GetConnectionString("Redis") ?? "localhost:6379" },
                    AbortOnConnectFail = false
                }));
            services.AddScoped<IRapportCache, RedisRapportCache>();
            services.AddScoped<INumeroRapportGenerator, RedisNumeroRapportGenerator>();
            return services;
        }

        public static IServiceCollection AddMessaging(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ConsultationTermineeConsumer>();
                x.AddConsumer<RdvConfirmeStatConsumer>();
                x.AddConsumer<RdvAnnuleStatConsumer>();
                x.AddConsumer<PatientCreeConsumer>();
                x.AddConsumer<PrescriptionSigneeStatConsumer>();
                x.AddConsumer<FactureGenereeConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config["RabbitMq:Host"] ?? "localhost", h =>
                    {
                        h.Username(config["RabbitMq:Username"] ?? "guest");
                        h.Password(config["RabbitMq:Password"] ?? "guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                    cfg.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15)));
                });
            });
            return services;
        }

        public static IServiceCollection AddHangfireJobs(
            this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(cfg => cfg.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
            services.AddScoped<ConsolidationSnapshotJob>();
            services.AddScoped<EnvoyerRapportsPlanifiesJob>();
            services.AddScoped<PurgerAnciensSnapshotsJob>();
            return services;
        }

        public static IServiceCollection AddJwtFromAuthSvc(
            this IServiceCollection services, IConfiguration config)
        {
            var jwksUri = config["Jwt:JwksUri"] ?? "http://auth-svc:5001/.well-known/jwks.json";
            var issuer = config["Jwt:Issuer"] ?? "DMI.Auth";
            var audience = config["Jwt:Audience"] ?? "DMI.Client";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        ClockSkew = TimeSpan.Zero,
                    };
                    opt.ConfigurationManager =
                        new ConfigurationManager<OpenIdConnectConfiguration>(
                            jwksUri,
                            new OpenIdConnectConfigurationRetriever(),
                            new HttpDocumentRetriever { RequireHttps = false });
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("MedecinOnly",
                    p => p.RequireRole("Medecin", "Admin"));
                opt.AddPolicy("StaffOnly",
                    p => p.RequireRole("Medecin", "Secretaire", "Admin"));
            });

            return services;
        }

        public static IServiceCollection AddValidatorsAndMapping(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<GenererRapportValidator>();
            return services;
        }
    }
}
