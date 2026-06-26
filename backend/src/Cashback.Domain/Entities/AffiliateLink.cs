using Cashback.Domain.Common;

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
    /// Primary tracking parameter sent to the affiliate provider.
    /// </summary>
    public string Sub1 { get; private set; } = null!;

    /// <summary>
    /// Affiliate campaign identifier.
    /// </summary>
    public string? CampaignId { get; private set; }

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

    /// <summary>
    /// Required by Entity Framework.
    /// </summary>
    private AffiliateLink()
    {
    }

    /// <summary>
    /// Creates a new affiliate link record.
    /// </summary>
    public static AffiliateLink Create(
        Guid userId,
        string originalUrl,
        string affiliateUrl,
        string? shortUrl,
        string sub1,
        string? campaignId)
    {
        return new AffiliateLink
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OriginalUrl = originalUrl,
            AffiliateUrl = affiliateUrl,
            ShortUrl = shortUrl,
            Sub1 = sub1,
            CampaignId = campaignId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
