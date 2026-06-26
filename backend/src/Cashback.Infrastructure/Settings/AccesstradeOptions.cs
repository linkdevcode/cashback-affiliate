namespace Cashback.Infrastructure.Settings;

/// <summary>
/// Accesstrade API configuration options.
/// </summary>
public sealed class AccesstradeOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Accesstrade";

    /// <summary>
    /// Accesstrade API base URL.
    /// </summary>
    public string BaseUrl { get; init; } = "https://api.accesstrade.vn";

    /// <summary>
    /// Publisher API access token.
    /// </summary>
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// Default campaign identifier for link generation.
    /// </summary>
    public string CampaignId { get; init; } = string.Empty;
}
