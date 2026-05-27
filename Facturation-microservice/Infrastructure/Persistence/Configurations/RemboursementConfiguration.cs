using Facturation_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturation_microservice.Infrastructure.Persistence.Configurations
{
    public class RemboursementConfiguration : IEntityTypeConfiguration<RemboursementCaisse>

    {
        public void Configure(EntityTypeBuilder<RemboursementCaisse> builder)
        {
            builder.HasIndex(x => x.Statut);

            builder.HasOne(x => x.Facture)
                .WithOne()
                .HasForeignKey<RemboursementCaisse>(x => x.FactureId);

            builder.HasIndex(x => x.FactureId)
                .IsUnique();
        }
    }
}
