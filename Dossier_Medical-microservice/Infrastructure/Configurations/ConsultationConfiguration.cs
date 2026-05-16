using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class ConsultationConfiguration
        : IEntityTypeConfiguration<Consultation>
    {
        public void Configure(EntityTypeBuilder<Consultation> builder)
        {
            builder.ToTable("Consultations");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Motif)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.Conclusion)
                   .HasMaxLength(4000);

            builder.Property(c => c.Statut)
                   .HasConversion<int>();

            builder.Property(c => c.TypeConsultation)
                   .HasConversion<int>();

            builder.HasIndex(c => c.DossierId);

            builder.HasIndex(c => c.Date);

            builder.Ignore(c => c.DomainEvents);

            builder.HasOne(c => c.DossierMedical)
                   .WithMany(d => d.Consultations)
                   .HasForeignKey(c => c.DossierId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Diagnostics)
                   .WithOne()
                   .HasForeignKey(d => d.ConsultationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Ordonnances)
                   .WithOne()
                   .HasForeignKey(o => o.ConsultationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(c => c.ExamenClinique, ec =>
            {
                ec.Property(x => x.Poids)
                  .HasPrecision(5, 2);

                ec.Property(x => x.Taille)
                  .HasPrecision(5, 2);

                ec.Property(x => x.Temperature)
                  .HasPrecision(4, 1);

                ec.Property(x => x.TA)
                  .HasMaxLength(20);

                ec.Property(x => x.Observations)
                  .HasMaxLength(2000);
            });
        }
    }
}