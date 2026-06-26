using Cashback.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="SystemSetting"/>.
/// </summary>
public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSettings");

        builder.HasKey(setting => setting.Id);

        builder.Property(setting => setting.Key)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(setting => setting.Value)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(setting => setting.Description)
            .HasColumnType("text");

        builder.Property(setting => setting.Category)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(setting => setting.UpdatedAt)
            .IsRequired();

        builder.HasIndex(setting => setting.Key)
            .IsUnique();

        builder.HasIndex(setting => setting.Category);
    }
}
