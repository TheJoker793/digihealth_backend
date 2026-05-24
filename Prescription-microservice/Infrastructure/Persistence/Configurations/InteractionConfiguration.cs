using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Infrastructure.Persistence.Configurations
{
    public class InteractionConfiguration : IEntityTypeConfiguration<InteractionDetectee>
    {
        public void Configure(EntityTypeBuilder<InteractionDetectee> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.PrescriptionId);
            builder.HasIndex(i => i.Severite);

            builder.Property(i => i.MedicamentA).IsRequired().HasMaxLength(200);
            builder.Property(i => i.MedicamentB).IsRequired().HasMaxLength(200);
            builder.Property(i => i.Mecanisme).IsRequired().HasMaxLength(500);
            builder.Property(i => i.Recommandation).IsRequired().HasMaxLength(500);
        }
    }
}




