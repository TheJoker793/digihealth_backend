using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Statistique_microservice.Infrastructure.Persistence
{
    public class StatistiqueDbContextFactory : IDesignTimeDbContextFactory<StatistiqueDbContext>
    {
        public StatistiqueDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

            var opt = new DbContextOptionsBuilder<StatistiqueDbContext>();
            opt.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(
                    typeof(StatistiqueDbContext).Assembly.FullName));

            return new StatistiqueDbContext(opt.Options);
        }
    }
}
