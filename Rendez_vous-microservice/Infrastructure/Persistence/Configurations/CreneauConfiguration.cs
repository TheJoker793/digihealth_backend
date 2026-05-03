using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Configurations
{
    public class CreneauConfiguration : IEntityTypeConfiguration<Creneau>
    {
        public void Configure(EntityTypeBuilder<Creneau> builder)
        {
            builder.HasKey(c => c.Id);

            // Index unique sur Debut + MedecinId
            builder.HasIndex(c => new { c.MedecinId, c.Debut })
                   .IsUnique();

            builder.HasIndex(c => c.EstDisponible);

            builder.Property(c => c.TypeCreneau)
                   .HasConversion<int>(); // enum -> int
        }
    }
}
