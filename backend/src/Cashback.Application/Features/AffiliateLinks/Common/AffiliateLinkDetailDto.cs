namespace Cashback.Application.Features.AffiliateLinks.Common;

/// <summary>
/// Detailed representation of an affiliate link.
/// </summary>
public sealed class AffiliateLinkDetailDto
{
    /// <summary>
    /// Affiliate link identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Original product URL submitted by the user.
    /// </summary>
    public string OriginalUrl { get; init; } = null!;

    /// <summary>
    /// Generated affiliate tracking URL.
    /// </summary>
    public string AffiliateUrl { get; init; } = null!;

    /// <summary>
    /// Shortened affiliate URL for sharing.
    /// </summary>
    public string? ShortUrl { get; init; }

    /// <summary>
    /// Primary tracking parameter sent to the affiliate provider.
    /// </summary>
    public string Sub1 { get; init; } = null!;

    /// <summary>
    /// Affiliate campaign identifier.
    /// </summary>
    public string? CampaignId { get; init; }

    /// <summary>
    /// UTC timestamp when the link was generated.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
