namespace Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;

/// <summary>
/// Accesstrade postback webhook payload.
/// </summary>
public sealed class AccesstradeWebhookRequest
{
    /// <summary>
    /// Provider order identifier.
    /// </summary>
    public string OrderId { get; init; } = string.Empty;

    /// <summary>
    /// Primary tracking parameter identifying the user.
    /// </summary>
    public string Sub1 { get; init; } = string.Empty;

    /// <summary>
    /// Commission amount reported by the provider.
    /// </summary>
    public decimal Commission { get; init; }

    /// <summary>
    /// Order status reported by the provider.
    /// </summary>
    public string Status { get; init; } = string.Empty;
}
