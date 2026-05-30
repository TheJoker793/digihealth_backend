using Document_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Document_microservice.Infrastructure.Persistence
{
    public class DocumentDbContext:DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) { }

        // ── DbSets ───────────────────────────────────────────────
        public DbSet<DocumentMedical> Documents => Set<DocumentMedical>();
        public DbSet<VersionDocument> Versions => Set<VersionDocument>();
        public DbSet<PartageDocument> Partages => Set<PartageDocument>();
        public DbSet<TemplateDocument> Templates => Set<TemplateDocument>();

        // ── Configuration ────────────────────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentDbContext).Assembly);
        }

        // ── Audit automatique ────────────────────────────────────
        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;
            }
            return await base.SaveChangesAsync(ct);
        }


    }
}
