using Microsoft.Extensions.DependencyInjection;

namespace Dossier_Medical_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // Domain services, validators, etc.
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Application layer services
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Infrastructure services (RabbitMQ, Minio, Redis, etc.)
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddJwtFromAuthSvc(this IServiceCollection services, IConfiguration config)
        {
            // Configure JWT validation via Auth.svc JWKS
            services.AddHttpClient("AuthSvc", client =>
            {
                client.BaseAddress = new Uri(config["Auth:BaseUrl"]);
            });

            return services;
        }
    }
}
