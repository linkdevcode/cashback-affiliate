using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for <see cref="AffiliateLink"/>.
/// </summary>
public class AffiliateLinkConfiguration : IEntityTypeConfiguration<AffiliateLink>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AffiliateLink> builder)
    {
        builder.ToTable("AffiliateLinks");

        builder.HasKey(link => link.Id);

        builder.Property(link => link.OriginalUrl)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(link => link.AffiliateUrl)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(link => link.ShortUrl)
            .HasColumnType("text");

        builder.Property(link => link.CampaignId)
            .HasMaxLength(100);

        builder.Property(link => link.SubId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(link => link.Merchant)
            .HasConversion<int>();

        builder.Property(link => link.CreatedAt)
            .IsRequired();

        builder.HasIndex(link => link.UserId);

        builder.HasIndex(link => link.SubId)
            .IsUnique();

        builder.HasOne(link => link.User)
            .WithMany(user => user.AffiliateLinks)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
