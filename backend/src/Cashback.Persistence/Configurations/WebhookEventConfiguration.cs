using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashback.Persistence.Configurations;

public class WebhookEventConfiguration : IEntityTypeConfiguration<WebhookEvent>
{
    public void Configure(EntityTypeBuilder<WebhookEvent> builder)
    {
        builder.ToTable("WebhookEvents");

        builder.HasKey(webhookEvent => webhookEvent.Id);

        builder.Property(webhookEvent => webhookEvent.Provider)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(webhookEvent => webhookEvent.EventId)
            .HasMaxLength(255);

        builder.Property(webhookEvent => webhookEvent.ProviderOrderId)
            .HasMaxLength(255);

        builder.Property(webhookEvent => webhookEvent.Payload)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(webhookEvent => webhookEvent.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(webhookEvent => webhookEvent.ErrorMessage)
            .HasColumnType("text");

        builder.Property(webhookEvent => webhookEvent.ReceivedAt)
            .IsRequired();

        builder.HasIndex(webhookEvent => webhookEvent.ProviderOrderId);

        builder.HasIndex(webhookEvent => webhookEvent.EventId);

        builder.HasIndex(webhookEvent => webhookEvent.Status);

        builder.HasIndex(webhookEvent => webhookEvent.ReceivedAt);
    }
}
