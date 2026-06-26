namespace Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;

/// <summary>
/// Accesstrade webhook acknowledgment response.
/// </summary>
public sealed class AccesstradeWebhookResponse
{
    /// <summary>
    /// Indicates the webhook payload was accepted.
    /// </summary>
    public bool Received { get; init; } = true;
}
