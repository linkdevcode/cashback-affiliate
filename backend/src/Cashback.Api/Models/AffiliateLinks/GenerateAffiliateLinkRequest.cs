namespace Cashback.Api.Models.AffiliateLinks;

/// <summary>
/// Request body for affiliate link generation.
/// </summary>
public sealed class GenerateAffiliateLinkRequest
{
    /// <summary>
    /// Shopee product URL to convert.
    /// </summary>
    public string Url { get; init; } = null!;
}
