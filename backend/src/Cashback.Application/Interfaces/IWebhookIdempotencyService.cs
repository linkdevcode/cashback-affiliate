namespace Cashback.Application.Interfaces;

/// <summary>
/// Coordinates webhook duplicate detection and processed event tracking.
/// </summary>
public interface IWebhookIdempotencyService
{
    /// <summary>
    /// Registers a webhook event or returns a duplicate result when already processed.
    /// </summary>
    Task<WebhookIdempotencyResult> RegisterOrGetDuplicateAsync(
        WebhookIdempotencyRequest request,
        CancellationToken cancellationToken);

    /// <summary>
    /// Marks a webhook event as successfully processed.
    /// </summary>
    Task MarkAsProcessedAsync(Guid webhookEventId, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a webhook event as failed with an error message.
    /// </summary>
    Task MarkAsFailedAsync(
        Guid webhookEventId,
        string errorMessage,
        CancellationToken cancellationToken);
}
