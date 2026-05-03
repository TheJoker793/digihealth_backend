using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Rendez_vous_microservice.Infrastructure.Persistence
{
    public class RendezVousDbContextFactory : IDesignTimeDbContextFactory<RendezVousDbContext>
    {
        public RendezVousDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RendezVousDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new RendezVousDbContext(optionsBuilder.Options);
        }
    }
}
