using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Infrastructure.Persistence.Configurations
{
    public class SnapshotActiviteConfiguration : IEntityTypeConfiguration<SnapshotActivite>
    {
        public void Configure(EntityTypeBuilder<SnapshotActivite> b)
        {
            b.ToTable("SnapshotsActivite");
            b.HasKey(s => s.Id);
            b.Property(s => s.ChiffreAffaires).HasPrecision(18, 3);
            b.Property(s => s.MontantImpaye).HasPrecision(18, 3);
            b.Property(s => s.TauxOccupation).HasPrecision(5, 2);

            // Index unique — un seul snapshot par cabinet par jour
            b.HasIndex(s => new { s.CabinetId, s.DateSnapshot }).IsUnique();
            b.HasIndex(s => new { s.CabinetId, s.EstConsolide });

            b.Ignore(s => s.DomainEvents);
        }
    }
}
