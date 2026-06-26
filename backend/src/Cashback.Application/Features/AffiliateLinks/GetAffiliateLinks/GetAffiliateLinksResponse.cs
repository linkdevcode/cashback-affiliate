using Cashback.Application.Features.AffiliateLinks.Common;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinks;

/// <summary>
/// Paginated affiliate link list response.
/// </summary>
public sealed class GetAffiliateLinksResponse
{
    /// <summary>
    /// Affiliate links for the current page.
    /// </summary>
    public IReadOnlyList<AffiliateLinkDto> Items { get; init; } = [];

    /// <summary>
    /// Total number of affiliate links for the user.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; }
}
