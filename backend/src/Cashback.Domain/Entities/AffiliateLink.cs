using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class AffiliateLink : BaseEntity
{
    public Guid UserId { get; private set; }

    public string OriginalUrl { get; private set; } = null!;

    public string AffiliateUrl { get; private set; } = null!;

    public string? ShortUrl { get; private set; }

    public string? CampaignId { get; private set; }

    public string SubId { get; private set; } = null!;

    public MerchantType? Merchant { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public ICollection<Order> Orders { get; private set; } = [];
}
