using Microsoft.EntityFrameworkCore;
using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Infrastructure.Persistence
{
    public class StatistiqueDbContext:DbContext
    {
        public StatistiqueDbContext(DbContextOptions<StatistiqueDbContext> options)
        : base(options) { }

        public DbSet<RapportStatistique> Rapports => Set<RapportStatistique>();
        public DbSet<IndicateurKPI> KPIs => Set<IndicateurKPI>();
        public DbSet<SnapshotActivite> Snapshots => Set<SnapshotActivite>();
        public DbSet<TableauDeBord> Tableaux => Set<TableauDeBord>();
        public DbSet<AbonnementRapport> Abonnements => Set<AbonnementRapport>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatistiqueDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Modified)
                    entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;

            return await base.SaveChangesAsync(ct);
        }

    }
}
