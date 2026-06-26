using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for affiliate links.
/// </summary>
public interface IAffiliateLinkRepository
{
    /// <summary>
    /// Gets an affiliate link by identifier.
    /// </summary>
    Task<AffiliateLink?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an affiliate link by identifier for a specific user.
    /// </summary>
    Task<AffiliateLink?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paginated list of affiliate links for a user.
    /// </summary>
    Task<(IReadOnlyList<AffiliateLink> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a new affiliate link.
    /// </summary>
    Task AddAsync(AffiliateLink affiliateLink, CancellationToken cancellationToken);
}
