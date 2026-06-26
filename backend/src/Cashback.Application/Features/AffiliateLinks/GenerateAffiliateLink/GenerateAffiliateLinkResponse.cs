namespace Cashback.Application.Features.AffiliateLinks.GenerateAffiliateLink;

/// <summary>
/// Response returned after a successful affiliate link generation.
/// </summary>
public sealed class GenerateAffiliateLinkResponse
{
    /// <summary>
    /// Generated affiliate link identifier.
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
    public string ShortUrl { get; init; } = null!;
}
