using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Infrastructure.Persistence
{
    public class RendezVousDbContext : DbContext
    {
        public RendezVousDbContext(DbContextOptions<RendezVousDbContext> options)
            : base(options) { }

        public DbSet<RendezVous> RendezVous { get; set; }
        public DbSet<Creneau> Creneaux { get; set; }
        public DbSet<RegleRecurrence> ReglesRecurrence { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
