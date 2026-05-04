using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Application.Services;
using Rendez_vous_microservice.Domain.Interfaces;
using Rendez_vous_microservice.Infrastructure.Persistence.Repositories;
using Rendez_vous_microservice.Infrastructure.Persistence.Services;
using Rendez_vous_microservice.Infrastructure.Persistence;
using DMI.RendezVous.Infrastructure.Persistence;
using Hangfire;

namespace Rendez_vous_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // Ici tu peux enregistrer tes services Domain si besoin
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Ici tu peux enregistrer tes services Application (cas d’usage, orchestrateurs)
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<RendezVousDbContext>(options =>
                options.UseSqlServer(connectionString));

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories (si tu veux les injecter directement)
            services.AddScoped<IRendezVousRepository, RendezVousRepository>();
            services.AddScoped<ICreneauRepository, CreneauRepository>();
            services.AddScoped<IRegleRecurrenceRepository, RegleRecurrenceRepository>();

            // Services techniques
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
            services.AddScoped<IRdvCacheService, RedisRdvCacheService>();
            services.AddScoped<IRdvNotifier, SignalRRdvNotifier>();
           // services.AddScoped<IRappelService, RappelService>(); // ton implémentation métier

            return services;
        }
        public static IServiceCollection AddHangfire(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
                config.UseSqlServerStorage(connectionString));

            services.AddHangfireServer();

            return services;
        }


        public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

    }
}
