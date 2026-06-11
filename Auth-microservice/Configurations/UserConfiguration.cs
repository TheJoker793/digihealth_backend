using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth_microservice.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Login)
            .IsUnique();

        builder.Property(u => u.HashedPassword)
            .IsRequired();

        // ✅ FIX
        builder.Property(u => u.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.CabinetId)
            .IsRequired();

        builder.Property(u => u.TotpSecret)
            .HasMaxLength(512);
    }
}