namespace Cashback.Application.Interfaces;

/// <summary>
/// Result of an idempotent webhook registration attempt.
/// </summary>
public sealed class WebhookIdempotencyResult
{
    /// <summary>
    /// Indicates whether the webhook was already processed and should be skipped.
    /// </summary>
    public bool IsDuplicate { get; init; }

    /// <summary>
    /// Identifier of the webhook event record.
    /// </summary>
    public Guid WebhookEventId { get; init; }

    /// <summary>
    /// Creates a duplicate result for an already processed webhook event.
    /// </summary>
    public static WebhookIdempotencyResult Duplicate(Guid webhookEventId)
    {
        return new WebhookIdempotencyResult
        {
            IsDuplicate = true,
            WebhookEventId = webhookEventId
        };
    }

    /// <summary>
    /// Creates a result for a webhook event accepted for processing.
    /// </summary>
    public static WebhookIdempotencyResult Accepted(Guid webhookEventId)
    {
        return new WebhookIdempotencyResult
        {
            IsDuplicate = false,
            WebhookEventId = webhookEventId
        };
    }
}
