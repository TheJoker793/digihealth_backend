using Dossier_Medical_microservice.Application.Services;
using Dossier_Medical_microservice.Domain.Interfaces;
using Dossier_Medical_microservice.Infrastructure.Persistence;
using Dossier_Medical_microservice.Infrastructure.Repositories;
using Dossier_Medical_microservice.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Minio;
using StackExchange.Redis;

namespace Dossier_Medical_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<DossierService>();
            services.AddScoped<ConsultationService>();
            services.AddScoped<DiagnosticService>();
            services.AddScoped<DocumentService>();
            services.AddScoped<OrdonnanceService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Database
            services.AddDbContext<DossierDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DossierDatabase")));

            // Repositories
            services.AddScoped<IDossierRepository, DossierRepository>();
            services.AddScoped<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IDiagnosticRepository, DiagnosticRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IOrdonnanceRepository, OrdonnanceRepository>();
            // Numero Dossier Generator
            services.AddScoped<INumeroDossierGenerator, NumeroDossierGenerator>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MinIO Client
            services.AddSingleton<MinioClient>(sp =>
            {
                var endpoint = config["MinIO:Endpoint"] ?? "http://minio:9000";
                var accessKey = config["MinIO:AccessKey"] ?? "minioadmin";
                var secretKey = config["MinIO:SecretKey"] ?? "minioadmin";
                var uri = new Uri(endpoint);
                var secure = uri.Scheme == "https";
                return (MinioClient)new MinioClient()
                    .WithEndpoint(uri.Host, uri.Port)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();
            });
            services.AddScoped<IStorageService, MinioStorageService>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var host = config["Redis:Host"] ?? "redis";
                var port = config["Redis:Port"] ?? "6379";
                return ConnectionMultiplexer.Connect($"{host}:{port}");
            });
            services.AddScoped<IDossierCacheService, RedisDossierCacheService>();

            // CIM11
            services.AddHttpClient<ICim11Service, Cim11HttpService>(client =>
            {
                client.BaseAddress = new Uri("http://cim11-api:80");
            });

            // Event Publisher (RabbitMQ via MassTransit)
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            // MassTransit
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = config["RabbitMQ:Host"] ?? "rabbitmq";
                    var port = ushort.Parse(config["RabbitMQ:Port"] ?? "5672");
                    var username = config["RabbitMQ:UserName"] ?? "guest";
                    var password = config["RabbitMQ:Password"] ?? "guest";
                    cfg.Host(host, port, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                    cfg.UseMessageRetry(r =>
                        r.Interval(5, TimeSpan.FromSeconds(5)));
                });
            });

            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddJwtFromAuthSvc(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
    }
}