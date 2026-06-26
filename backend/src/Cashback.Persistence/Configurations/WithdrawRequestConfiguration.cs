using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="WithdrawRequest"/>.
/// </summary>
public class WithdrawRequestConfiguration : IEntityTypeConfiguration<WithdrawRequest>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<WithdrawRequest> builder)
    {
        builder.ToTable("WithdrawRequests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(request => request.BankName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(request => request.BankAccountNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(request => request.BankAccountName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(request => request.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Note)
            .HasColumnType("text");

        builder.Property(request => request.RequestedAt)
            .IsRequired();

        builder.HasIndex(request => request.UserId);

        builder.HasIndex(request => request.Status);

        builder.HasOne(request => request.User)
            .WithMany(user => user.WithdrawRequests)
            .HasForeignKey(request => request.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
