namespace Cashback.Application.Interfaces;

/// <summary>
/// Affiliate link generation provider abstraction.
/// </summary>
public interface IAffiliateProvider
{
    /// <summary>
    /// Generates an affiliate tracking link for the given product URL.
    /// </summary>
    Task<AffiliateLinkResult?> GenerateAffiliateLinkAsync(
        AffiliateLinkGenerationRequest request,
        CancellationToken cancellationToken);
}
