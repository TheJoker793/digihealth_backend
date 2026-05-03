using Patient_microservice.Services;

namespace Patient_microservice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<PatientService>();
            services.AddScoped<AssuranceService>();
            services.AddScoped<AntecedentService>();
            services.AddScoped<ContactUrgenceService>();
            services.AddScoped<CouvertureSocialService>();
            services.AddScoped<DoublonService>();
            services.AddScoped<PieceIdentiteService>();




            return services;
        }
    }
}
