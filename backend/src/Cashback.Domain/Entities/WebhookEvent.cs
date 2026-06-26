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
    public string ProviderOrderId { get; private set; } = null!;

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

    /// <summary>
    /// Indicates whether the webhook event has already been processed.
    /// </summary>
    public bool IsAlreadyProcessed =>
        Status is WebhookEventStatus.Processed or WebhookEventStatus.Ignored;

    /// <summary>
    /// Required by Entity Framework.
    /// </summary>
    private WebhookEvent()
    {
    }

    /// <summary>
    /// Creates a new webhook event in received status.
    /// </summary>
    public static WebhookEvent Create(
        string provider,
        string providerOrderId,
        string payload,
        string? eventId = null)
    {
        return new WebhookEvent
        {
            Id = Guid.NewGuid(),
            Provider = provider,
            ProviderOrderId = providerOrderId,
            EventId = eventId,
            Payload = payload,
            Status = WebhookEventStatus.Received,
            ReceivedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Marks the webhook event as successfully processed.
    /// </summary>
    public void MarkAsProcessed()
    {
        Status = WebhookEventStatus.Processed;
        ProcessedAt = DateTime.UtcNow;
        ErrorMessage = null;
    }

    /// <summary>
    /// Marks the webhook event as currently being processed.
    /// </summary>
    public void MarkAsProcessing()
    {
        Status = WebhookEventStatus.Processing;
    }

    /// <summary>
    /// Marks the webhook event as failed with an error message.
    /// </summary>
    public void MarkAsFailed(string errorMessage)
    {
        Status = WebhookEventStatus.Failed;
        ErrorMessage = errorMessage;
    }
}
