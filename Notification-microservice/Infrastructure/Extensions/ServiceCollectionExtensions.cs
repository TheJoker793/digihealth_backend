using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification_microservice.Application.Mappings;
using Notification_microservice.Application.Services;
using Notification_microservice.Application.Validators;
using Notification_microservice.Domain.Interfaces;
using Notification_microservice.Infrastructure.Jobs;
using Notification_microservice.Infrastructure.Messaging;
using Notification_microservice.Infrastructure.Persistence.Repositories;
using Notification_microservice.Infrastructure.Persistence;
using Notification_microservice.Infrastructure.Rendering;
using Notification_microservice.Infrastructure.Senders;
using StackExchange.Redis;
using Notification_microservice.Application.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using FluentValidation;
using Hangfire;
using SendGrid.Extensions.DependencyInjection;






namespace Notification_microservice.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // ── Base de données ──────────────────────────────────────
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NotificationDbContext>(opt =>
                opt.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(
                        typeof(NotificationDbContext).Assembly.FullName)));
            return services;
        }

        // ── Repositories + UnitOfWork ────────────────────────────
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ITemplateNotificationRepository, TemplateNotificationRepository>();
            services.AddScoped<IPreferenceNotificationRepository, PreferenceNotificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        // ── Services Application ─────────────────────────────────
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<NotificationService>();
            services.AddScoped<DispatchService>();
            services.AddScoped<TemplateService>();
            services.AddScoped<PreferenceService>();
            return services;
        }

        // ── Providers d'envoi ────────────────────────────────────
        public static IServiceCollection AddSenders(
            this IServiceCollection services, IConfiguration config)
        {
            // SendGrid
            services.AddSendGrid(opt =>
                opt.ApiKey = config["SendGrid:ApiKey"]
                    ?? throw new InvalidOperationException("SendGrid:ApiKey manquant."));
            services.AddScoped<IEmailSender, SendGridEmailSender>();

            // Twilio SMS
            services.AddScoped<ISmsSender, TwilioSmsSender>();

            // Firebase FCM
            var firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(
                    config["Firebase:CredentialPath"]
                        ?? throw new InvalidOperationException("Firebase:CredentialPath manquant."))
            });
            services.AddSingleton(firebaseApp);
            services.AddScoped<IPushSender, FcmPushSender>();

            return services;
        }

        // ── Redis ────────────────────────────────────────────────
        public static IServiceCollection AddRedis(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { config.GetConnectionString("Redis") ?? "localhost:6379" },
                    AbortOnConnectFail = false
                }));
            services.AddScoped<INumeroNotificationGenerator, RedisNumeroNotificationGenerator>();
            return services;
        }

        // ── Template renderer ────────────────────────────────────
        public static IServiceCollection AddRendering(this IServiceCollection services)
        {
            services.AddScoped<ITemplateRenderer, ScribanTemplateRenderer>();
            return services;
        }

        // ── RabbitMQ (MassTransit) ───────────────────────────────
        public static IServiceCollection AddMessaging(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            services.AddMassTransit(x =>
            {
                // Enregistrer tous les consumers
                x.AddConsumer<RdvConfirmeConsumer>();
                x.AddConsumer<RdvRappelConsumer>();
                x.AddConsumer<RdvAnnuleConsumer>();
                x.AddConsumer<DocumentPublieConsumer>();
                x.AddConsumer<OrdonnanceSigneeConsumer>();

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
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                });
            });

            return services;
        }

        // ── Hangfire ─────────────────────────────────────────────
        public static IServiceCollection AddHangfireJobs(
            this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(cfg => cfg.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
            services.AddScoped<RetryNotificationJob>();
            services.AddScoped<EnvoyerProgrammeesJob>();
            services.AddScoped<CleanupHistoriqueJob>();
            return services;
        }

        // ── JWT (JWKS depuis Auth.svc) ───────────────────────────
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
                opt.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
                opt.AddPolicy("StaffOnly", p => p.RequireRole("Medecin", "Secretaire", "Admin"));
            });

            return services;
        }

        // ── Validators + AutoMapper ──────────────────────────────
        public static IServiceCollection AddValidatorsAndMapping(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<EnvoyerNotificationValidator>();
            services.AddAutoMapper(typeof(NotificationMappingProfile));
            return services;
        }
    }
}
