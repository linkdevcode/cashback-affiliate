using System.Text.Json.Serialization;

namespace Cashback.Infrastructure.Providers.Accesstrade;

/// <summary>
/// Accesstrade product link creation request payload.
/// </summary>
internal sealed class AccesstradeCreateProductLinkRequest
{
    /// <summary>
    /// Campaign identifier.
    /// </summary>
    [JsonPropertyName("campaign_id")]
    public string CampaignId { get; init; } = null!;

    /// <summary>
    /// Product URLs to convert.
    /// </summary>
    [JsonPropertyName("urls")]
    public IReadOnlyList<string> Urls { get; init; } = [];

    /// <summary>
    /// Primary tracking parameter.
    /// </summary>
    [JsonPropertyName("sub1")]
    public string? Sub1 { get; init; }

    /// <summary>
    /// Secondary tracking parameter.
    /// </summary>
    [JsonPropertyName("sub2")]
    public string? Sub2 { get; init; }

    /// <summary>
    /// Tertiary tracking parameter.
    /// </summary>
    [JsonPropertyName("sub3")]
    public string? Sub3 { get; init; }
}

/// <summary>
/// Accesstrade product link creation response payload.
/// </summary>
internal sealed class AccesstradeCreateProductLinkResponse
{
    /// <summary>
    /// Indicates whether the API call succeeded.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    /// <summary>
    /// Response data container.
    /// </summary>
    [JsonPropertyName("data")]
    public AccesstradeCreateProductLinkData? Data { get; init; }
}

/// <summary>
/// Accesstrade product link creation response data.
/// </summary>
internal sealed class AccesstradeCreateProductLinkData
{
    /// <summary>
    /// Successfully generated links.
    /// </summary>
    [JsonPropertyName("success_link")]
    public IReadOnlyList<AccesstradeSuccessLink> SuccessLinks { get; init; } = [];

    /// <summary>
    /// Links that failed to generate.
    /// </summary>
    [JsonPropertyName("error_link")]
    public IReadOnlyList<string> ErrorLinks { get; init; } = [];
}

/// <summary>
/// A successfully generated Accesstrade tracking link.
/// </summary>
internal sealed class AccesstradeSuccessLink
{
    /// <summary>
    /// Full affiliate tracking URL.
    /// </summary>
    [JsonPropertyName("aff_link")]
    public string? AffiliateLink { get; init; }

    /// <summary>
    /// Shortened affiliate URL.
    /// </summary>
    [JsonPropertyName("short_link")]
    public string? ShortLink { get; init; }

    /// <summary>
    /// Original product URL.
    /// </summary>
    [JsonPropertyName("url_origin")]
    public string? UrlOrigin { get; init; }
}
