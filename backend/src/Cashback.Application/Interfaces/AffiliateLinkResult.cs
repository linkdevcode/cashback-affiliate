namespace Cashback.Application.Interfaces;

/// <summary>
/// Generated affiliate link details from a provider.
/// </summary>
public sealed class AffiliateLinkResult
{
    /// <summary>
    /// Original product URL submitted for conversion.
    /// </summary>
    public string OriginalUrl { get; init; } = null!;

    /// <summary>
    /// Full affiliate tracking URL.
    /// </summary>
    public string AffiliateUrl { get; init; } = null!;

    /// <summary>
    /// Shortened affiliate URL.
    /// </summary>
    public string ShortUrl { get; init; } = null!;

    /// <summary>
    /// Campaign identifier used for link generation.
    /// </summary>
    public string? CampaignId { get; init; }
}
