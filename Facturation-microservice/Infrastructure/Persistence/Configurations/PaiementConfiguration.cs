using Facturation_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturation_microservice.Infrastructure.Persistence.Configurations
{
    public class PaiementConfiguration : IEntityTypeConfiguration<Paiement>
    {
        public void Configure(EntityTypeBuilder<Paiement> builder)
        {
            builder.HasIndex(x => x.Date);

            builder.Property(x => x.Montant)
                .HasPrecision(10, 3);

            builder.HasOne<Facture>()
                .WithMany()
                .HasForeignKey(x => x.FactureId);
        }
    }
}
