using Document_microservice.Application.Services;
using Document_microservice.Domain.Interfaces;
using Document_microservice.Infrastructure.Persistence;
using Document_microservice.Services;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Extensions
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
            services.AddDbContext<DocumentDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sql => sql.MigrationsAssembly(
                        typeof(DocumentDbContext).Assembly.FullName)));

            return services;
        }
        // =========================
        // Application UNIT OF WORK
        //
        public static IServiceCollection AddUnitOfWork(
             this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        // =========================
        // Application Service
        //
        public static IServiceCollection AddApplicationServices(
    this IServiceCollection services)
        {
            services.AddScoped<DocumentService>();
            services.AddScoped<GenerationPdfService>();
            services.AddScoped<PartageService>();
            services.AddScoped<TemplateService>();
            services.AddScoped<VersionService>();
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration) 
        {
            services.AddScoped<IDocumentCacheService,DocumentCacheService>();
            services.AddScoped<IStorageService, MinioStorageService>();
            services.AddScoped<INumeroDocumentGenerator, NumeroDocumentGenerator>();
            services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
            return services;
        }

    }
}
