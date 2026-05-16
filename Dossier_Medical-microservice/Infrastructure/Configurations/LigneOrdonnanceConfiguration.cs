using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class LigneOrdonnanceConfiguration : IEntityTypeConfiguration<LigneOrdonnance>
    {
        public void Configure(EntityTypeBuilder<LigneOrdonnance> builder)
        {
            builder.ToTable("LignesOrdonnance");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.NomMedicament)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Posologie)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasIndex(l => l.OrdonnanceId);

            builder.HasOne(l => l.Ordonnance)
                   .WithMany(o => o.Lignes)
                   .HasForeignKey(l => l.OrdonnanceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(l => l.DomainEvents);
        }
    }
}