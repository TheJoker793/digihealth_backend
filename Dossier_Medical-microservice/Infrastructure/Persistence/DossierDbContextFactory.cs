using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dossier_Medical_microservice.Infrastructure.Persistence
{
    public class DossierDbContextFactory
        : IDesignTimeDbContextFactory<DossierDbContext>
    {
        public DossierDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DossierDbContext>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(DossierDbContext).Assembly.FullName)
            );

            return new DossierDbContext(optionsBuilder.Options);
        }
    }
}