using Cashback.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Cashback.Infrastructure.Settings;

/// <summary>
/// Accesstrade webhook settings backed by configuration options.
/// </summary>
public sealed class AccesstradeWebhookSettings : IAccesstradeWebhookSettings
{
    /// <summary>
    /// Initializes webhook settings from Accesstrade configuration.
    /// </summary>
    public AccesstradeWebhookSettings(IOptions<AccesstradeOptions> options)
    {
        WebhookSecret = options.Value.WebhookSecret;
    }

    /// <inheritdoc/>
    public string WebhookSecret { get; }
}
