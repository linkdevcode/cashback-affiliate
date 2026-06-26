namespace Cashback.Application.Interfaces;

/// <summary>
/// Input for registering a webhook event with idempotency checks.
/// </summary>
public sealed class WebhookIdempotencyRequest
{
    /// <summary>
    /// Webhook provider identifier.
    /// </summary>
    public string Provider { get; init; } = string.Empty;

    /// <summary>
    /// Provider order identifier used for duplicate detection.
    /// </summary>
    public string ProviderOrderId { get; init; } = string.Empty;

    /// <summary>
    /// Optional provider-specific event identifier.
    /// </summary>
    public string? EventId { get; init; }

    /// <summary>
    /// Raw webhook payload body.
    /// </summary>
    public string Payload { get; init; } = string.Empty;
}
