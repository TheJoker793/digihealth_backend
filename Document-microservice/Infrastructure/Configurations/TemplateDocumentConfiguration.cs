using Document_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Document_microservice.Infrastructure.Configurations
{
    public class TemplateDocumentConfiguration : IEntityTypeConfiguration<TemplateDocument>
    {
        public void Configure(EntityTypeBuilder<TemplateDocument> builder)
        {
            builder.ToTable("TemplatesDocument");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nom)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.TypeDocument)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(t => t.Specialite).HasMaxLength(50);
            builder.Property(t => t.Version).HasMaxLength(10);

            builder.Property(t => t.ContenuHtml)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // string[] Variables → JSON
            builder.Property(t => t.Variables)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .HasColumnName("Variables")
                .HasMaxLength(2000);

            builder.HasIndex(t => new { t.TypeDocument, t.Specialite, t.EstActif });
            builder.HasIndex(t => new { t.CabinetId, t.EstActif });

            builder.Ignore(t => t.DomainEvents);
        }
    }
}
