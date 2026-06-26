namespace Cashback.Application.Features.AffiliateLinks.Common;

/// <summary>
/// Summary representation of an affiliate link for list responses.
/// </summary>
public sealed class AffiliateLinkDto
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
    /// UTC timestamp when the link was generated.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
