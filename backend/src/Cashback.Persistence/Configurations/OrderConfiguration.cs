using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="Order"/>.
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.NetworkOrderId)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(order => order.Merchant)
            .HasConversion<int>();

        builder.Property(order => order.OrderAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(order => order.CommissionAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(order => order.CashbackAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(order => order.PlatformProfit)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.Property(order => order.UpdatedAt)
            .IsRequired();

        builder.HasIndex(order => order.UserId);

        builder.HasIndex(order => order.NetworkOrderId)
            .IsUnique();

        builder.HasIndex(order => order.Status);

        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(order => order.AffiliateLink)
            .WithMany(link => link.Orders)
            .HasForeignKey(order => order.AffiliateLinkId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
