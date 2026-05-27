using Facturation_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Facturation_microservice.Infrastructure.Persistence
{
    public class FacturationDbContext:DbContext
    {
        public FacturationDbContext(DbContextOptions<FacturationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Facture> Factures => Set<Facture>();

        public DbSet<LigneFacture> LignesFacture => Set<LigneFacture>();

        public DbSet<Paiement> Paiements => Set<Paiement>();

        public DbSet<RemboursementCaisse> Remboursements => Set<RemboursementCaisse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(FacturationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
