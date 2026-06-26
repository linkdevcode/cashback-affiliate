namespace Cashback.Application.Interfaces;

/// <summary>
/// Context for validating an incoming webhook request.
/// </summary>
public sealed class WebhookValidationContext
{
    /// <summary>
    /// Webhook provider identifier derived from the request path.
    /// </summary>
    public string Provider { get; init; } = string.Empty;

    /// <summary>
    /// Raw request body used for signature verification.
    /// </summary>
    public string RawBody { get; init; } = string.Empty;

    /// <summary>
    /// Shared secret supplied by the provider.
    /// </summary>
    public string? Secret { get; init; }

    /// <summary>
    /// Signature supplied by the provider.
    /// </summary>
    public string? Signature { get; init; }

    /// <summary>
    /// Request path that received the webhook.
    /// </summary>
    public string Path { get; init; } = string.Empty;
}
