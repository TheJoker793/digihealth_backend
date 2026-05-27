using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Facturation_microservice.Infrastructure.Persistence
{
    public class FacturationDbContextFactory : IDesignTimeDbContextFactory<FacturationDbContext>
    {
        public FacturationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder =
                new DbContextOptionsBuilder<FacturationDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new FacturationDbContext(optionsBuilder.Options);
        }
    }
}
