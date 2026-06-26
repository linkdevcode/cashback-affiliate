namespace Cashback.Domain.Enums;

/// <summary>
/// Webhook event processing status.
/// </summary>
public enum WebhookEventStatus
{
    /// <summary>
    /// Webhook event has been received.
    /// </summary>
    Received = 1,

    /// <summary>
    /// Webhook event is being processed.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Webhook event was processed successfully.
    /// </summary>
    Processed = 3,

    /// <summary>
    /// Webhook event processing failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Webhook event was ignored as a duplicate or invalid event.
    /// </summary>
    Ignored = 5
}
