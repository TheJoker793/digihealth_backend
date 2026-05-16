using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dossier_Medical_microservice.Infrastructure.Persistence
{
    public class DossierDbContext : DbContext
    {
        public DossierDbContext(DbContextOptions<DossierDbContext> options)
            : base(options)
        {
        }

        public DbSet<DossierMedical> Dossiers { get; set; } = default!;
        public DbSet<Consultation> Consultations { get; set; } = default!;
        public DbSet<Diagnostic> Diagnostics { get; set; } = default!;
        public DbSet<Ordonnance> Ordonnances { get; set; } = default!;
        public DbSet<LigneOrdonnance> LignesOrdonnances { get; set; } = default!;
        public DbSet<DocumentMedical> Documents { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ⭐ IMPORTANT : charge toutes les configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DossierDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                else if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}