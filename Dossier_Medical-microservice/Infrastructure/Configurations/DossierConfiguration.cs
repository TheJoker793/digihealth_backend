using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class DossierConfiguration : IEntityTypeConfiguration<DossierMedical>
    {
        public void Configure(EntityTypeBuilder<DossierMedical> builder)
        {
            builder.ToTable("Dossiers");

            builder.HasKey(d => d.Id);

            builder.OwnsOne(d => d.NumeroDossier, nd =>
            {
                nd.Property(p => p.Valeur)
                  .HasColumnName("NumeroDossier")
                  .IsRequired()
                  .HasMaxLength(30);

                nd.HasIndex(p => p.Valeur)
                  .IsUnique();
            });

            builder.Property(d => d.Motif)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.Anamnese)
                   .HasMaxLength(4000);

            builder.Property(d => d.Statut)
                   .HasConversion<int>();

            builder.HasIndex(d => d.PatientId);
            builder.HasIndex(d => d.MedecinId);

            builder.HasIndex(d => new
            {
                d.PatientId,
                d.MedecinId
            });

            builder.Ignore(d => d.DomainEvents);

            builder.HasMany(d => d.Consultations)
                   .WithOne()
                   .HasForeignKey(c => c.DossierId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Documents)
                   .WithOne()
                   .HasForeignKey(doc => doc.DossierId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}