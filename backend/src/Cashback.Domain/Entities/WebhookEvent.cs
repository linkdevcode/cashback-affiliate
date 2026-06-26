using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class WebhookEvent : BaseEntity
{
    public string Provider { get; private set; } = null!;

    public string? EventId { get; private set; }

    public string? ProviderOrderId { get; private set; }

    public string Payload { get; private set; } = null!;

    public WebhookEventStatus Status { get; private set; }

    public string? ErrorMessage { get; private set; }

    public DateTime ReceivedAt { get; private set; }

    public DateTime? ProcessedAt { get; private set; }
}
