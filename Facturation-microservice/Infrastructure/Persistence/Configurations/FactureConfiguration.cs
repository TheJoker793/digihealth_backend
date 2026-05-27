using Facturation_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturation_microservice.Infrastructure.Persistence.Configurations
{
    public class FactureConfiguration : IEntityTypeConfiguration<Facture>
    {
        public void Configure(EntityTypeBuilder<Facture> builder)
        {
            builder.HasIndex(x => x.PatientId);

            builder.HasIndex(x => x.NumeroFacture)
                .IsUnique();

            builder.Property(x => x.TauxTVA)
                .HasPrecision(10, 3);

            builder.Property(x => x.Remise)
                .HasPrecision(10, 3);
        }
    }
}
