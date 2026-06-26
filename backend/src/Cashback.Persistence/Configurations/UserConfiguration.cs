using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="User"/>.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnType("text");

        builder.Property(user => user.FullName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.AvatarUrl)
            .HasColumnType("text");

        builder.Property(user => user.Provider)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(user => user.ProviderUserId)
            .HasMaxLength(255);

        builder.Property(user => user.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(user => user.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(user => user.EmailVerified)
            .IsRequired();

        builder.Property(user => user.AvailableBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(user => user.PendingBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(user => user.LifetimeCashback)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(user => user.LastLoginAt);

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.ProviderUserId);

        builder.HasIndex(user => user.Role);

        builder.HasIndex(user => user.Status);
    }
}
