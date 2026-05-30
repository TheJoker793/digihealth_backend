using Document_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Document_microservice.Infrastructure.Configurations
{
    public class PartageDocumentConfiguration : IEntityTypeConfiguration<PartageDocument>
    {
        public void Configure(EntityTypeBuilder<PartageDocument> builder)
        {
            builder.ToTable("PartagesDocument");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TokenAcces)
                .IsRequired()
                .HasMaxLength(36);

            builder.HasIndex(p => p.TokenAcces).IsUnique();

            builder.Property(p => p.TypeDestinataire)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(p => p.DestinataireId);
            builder.HasIndex(p => new { p.DocumentMedicalId, p.EstRevoque });

            builder.Ignore(p => p.DomainEvents);
        }
    }
}
