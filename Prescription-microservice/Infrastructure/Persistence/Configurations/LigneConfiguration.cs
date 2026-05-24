using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.ValueObjects;

namespace Prescription_microservice.Infrastructure.Persistence.Configurations
{
    public class LigneConfiguration : IEntityTypeConfiguration<LignePrescription>
    {
        public void Configure(EntityTypeBuilder<LignePrescription> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasIndex(l => l.PrescriptionId);

            builder.Property(l => l.NomMedicament).IsRequired().HasMaxLength(200);
            builder.Property(l => l.DCI).IsRequired().HasMaxLength(200);

            // Value Object Posologie
            builder.OwnsOne(l => l.Posologie, pos =>
            {
                pos.Property(p => p.Dose).HasColumnName("Dose").IsRequired();
                pos.Property(p => p.Unite).HasColumnName("Unite").IsRequired();
                pos.Property(p => p.Frequence).HasColumnName("Frequence").IsRequired();
                pos.Property(p => p.Moment).HasColumnName("Moment").IsRequired();
            });
        }
    }
}
