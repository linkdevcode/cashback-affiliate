using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// Stored webhook event received from an affiliate provider.
/// </summary>
public class WebhookEvent : BaseEntity
{
    /// <summary>
    /// Name of the webhook provider.
    /// </summary>
    public string Provider { get; private set; } = null!;

    /// <summary>
    /// Provider-specific event identifier.
    /// </summary>
    public string? EventId { get; private set; }

    /// <summary>
    /// Provider order identifier referenced by the event.
    /// </summary>
    public string? ProviderOrderId { get; private set; }

    /// <summary>
    /// Raw webhook payload body.
    /// </summary>
    public string Payload { get; private set; } = null!;

    /// <summary>
    /// Current processing status of the webhook event.
    /// </summary>
    public WebhookEventStatus Status { get; private set; }

    /// <summary>
    /// Error message captured when processing fails.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// UTC timestamp when the webhook was received.
    /// </summary>
    public DateTime ReceivedAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the webhook was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; private set; }
}
