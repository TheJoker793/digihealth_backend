using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class OrdonnanceConfiguration : IEntityTypeConfiguration<Ordonnance>
    {
        public void Configure(EntityTypeBuilder<Ordonnance> builder)
        {
            builder.ToTable("Ordonnances");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Instructions)
                   .HasMaxLength(2000);

            builder.Property(o => o.ValiditeJours)
                   .IsRequired();

            builder.HasIndex(o => o.ConsultationId);

            builder.Ignore(o => o.DomainEvents);

            builder.HasOne(o => o.Consultation)
                   .WithMany(c => c.Ordonnances)
                   .HasForeignKey(o => o.ConsultationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Lignes)
                   .WithOne(l => l.Ordonnance)
                   .HasForeignKey(l => l.OrdonnanceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}