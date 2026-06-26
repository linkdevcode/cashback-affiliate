using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="CommissionTransaction"/>.
/// </summary>
public class CommissionTransactionConfiguration : IEntityTypeConfiguration<CommissionTransaction>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CommissionTransaction> builder)
    {
        builder.ToTable("CommissionTransactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(transaction => transaction.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(transaction => transaction.Description)
            .HasColumnType("text");

        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired();

        builder.HasIndex(transaction => transaction.UserId);

        builder.HasIndex(transaction => transaction.OrderId);

        builder.HasOne(transaction => transaction.User)
            .WithMany(user => user.CommissionTransactions)
            .HasForeignKey(transaction => transaction.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(transaction => transaction.Order)
            .WithMany(order => order.CommissionTransactions)
            .HasForeignKey(transaction => transaction.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
