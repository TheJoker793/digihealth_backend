using Facturation_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturation_microservice.Infrastructure.Persistence.Configurations
{
    public class LigneFactureConfiguration : IEntityTypeConfiguration<LigneFacture>
    {
        public void Configure(EntityTypeBuilder<LigneFacture> builder)
        {
            builder.HasOne<Facture>()
                .WithMany(x => x.LignesFacture)
                .HasForeignKey(x => x.FactureId);

            builder.HasIndex(x => x.TypeActe);

            builder.Property(x => x.PrixUnitaire)
                .HasPrecision(10, 3);
        }
    }
}
