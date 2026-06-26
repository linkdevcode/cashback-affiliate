using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// Generated affiliate tracking link for a product URL.
/// </summary>
public class AffiliateLink : BaseEntity
{
    /// <summary>
    /// Owner of the affiliate link.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Original product or campaign URL provided by the user.
    /// </summary>
    public string OriginalUrl { get; private set; } = null!;

    /// <summary>
    /// Affiliate tracking URL returned by the provider.
    /// </summary>
    public string AffiliateUrl { get; private set; } = null!;

    /// <summary>
    /// Shortened URL for sharing.
    /// </summary>
    public string? ShortUrl { get; private set; }

    /// <summary>
    /// Affiliate campaign identifier.
    /// </summary>
    public string? CampaignId { get; private set; }

    /// <summary>
    /// Tracking sub-identifier used to map conversions to users.
    /// </summary>
    public string SubId { get; private set; } = null!;

    /// <summary>
    /// Merchant associated with the link.
    /// </summary>
    public MerchantType? Merchant { get; private set; }

    /// <summary>
    /// UTC timestamp when the link was generated.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// User who generated the link.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Orders attributed to this affiliate link.
    /// </summary>
    public ICollection<Order> Orders { get; private set; } = [];
}
