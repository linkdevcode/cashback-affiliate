using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="AuditLog"/>.
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(auditLog => auditLog.Id);

        builder.Property(auditLog => auditLog.Action)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(auditLog => auditLog.EntityName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(auditLog => auditLog.Metadata)
            .HasColumnType("text");

        builder.Property(auditLog => auditLog.CreatedAt)
            .IsRequired();

        builder.HasIndex(auditLog => auditLog.UserId);
        builder.HasIndex(auditLog => auditLog.EntityName);
        builder.HasIndex(auditLog => auditLog.CreatedAt);
    }
}
