namespace Cashback.Application.Interfaces;

/// <summary>
/// Accesstrade webhook configuration settings.
/// </summary>
public interface IAccesstradeWebhookSettings
{
    /// <summary>
    /// Shared secret used to validate incoming webhook requests.
    /// </summary>
    string WebhookSecret { get; }
}
