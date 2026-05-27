using Facturation_microservice.Application.Services;
using Facturation_microservice.Domain.Interfaces;
using Facturation_microservice.Infrastructure.Persistence;
using Facturation_microservice.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Hangfire;
using Facturation_microservice.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Facturation_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<FacturationDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<FacturationService>();
            services.AddScoped<RemboursementService>();
            services.AddScoped<RelanceService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
            services.AddScoped<IPdfService, QuestPdfService>();
            services.AddScoped<ICnamService, CnamHttpService>();
            services.AddScoped<INumeroFactureGenerator, NumeroFactureGenerator>();
            services.AddScoped<IFactureCacheService, RedisFactureCacheService>();

            return services;
        }

        public static IServiceCollection AddRedis(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    config["Redis:ConnectionString"]!));

            return services;
        }

        public static IServiceCollection AddHttpClients(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddHttpClient<ICnamService, CnamHttpService>(client =>
            {
                client.BaseAddress = new Uri(config["Cnam:BaseUrl"]!);
            });

            return services;
        }

        public static IServiceCollection AddHangfire(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddHangfire(cfg =>
            {
                cfg.UseSqlServerStorage(
                    config.GetConnectionString("DefaultConnection"));
            });

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}