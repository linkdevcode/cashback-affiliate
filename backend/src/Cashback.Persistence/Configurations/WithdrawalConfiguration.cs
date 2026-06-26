using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="Withdrawal"/>.
/// </summary>
public class WithdrawalConfiguration : IEntityTypeConfiguration<Withdrawal>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Withdrawal> builder)
    {
        builder.ToTable("Withdrawals");

        builder.HasKey(withdrawal => withdrawal.Id);

        builder.Property(withdrawal => withdrawal.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(withdrawal => withdrawal.BankName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(withdrawal => withdrawal.BankAccountNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(withdrawal => withdrawal.BankAccountHolder)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(withdrawal => withdrawal.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(withdrawal => withdrawal.RequestedAt)
            .IsRequired();

        builder.HasIndex(withdrawal => withdrawal.UserId);
        builder.HasIndex(withdrawal => withdrawal.Status);
        builder.HasIndex(withdrawal => withdrawal.RequestedAt);

        builder.HasOne(withdrawal => withdrawal.User)
            .WithMany(user => user.Withdrawals)
            .HasForeignKey(withdrawal => withdrawal.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
