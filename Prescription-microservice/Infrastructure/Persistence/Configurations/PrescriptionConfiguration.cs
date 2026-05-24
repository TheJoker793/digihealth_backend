using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Infrastructure.Persistence.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.PatientId);
            builder.HasIndex(p => p.NumeroPrescriptionGuid).IsUnique();

            // Value Object NumeroPrescription
            builder.OwnsOne(p => p.NumeroPrescription, np =>
            {
                np.Property(n => n.Valeur)
                  .HasColumnName("NumeroPrescription")
                  .IsRequired();
            });

            builder.Property(p => p.Date).IsRequired();
            builder.Property(p => p.ValiditeJours).IsRequired();
            builder.Property(p => p.Statut).IsRequired();
        }
    }
}
