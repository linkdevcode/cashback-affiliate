using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class Order : AuditableEntity
{
    public Guid UserId { get; private set; }

    public Guid? AffiliateLinkId { get; private set; }

    public string NetworkOrderId { get; private set; } = null!;

    public MerchantType? Merchant { get; private set; }

    public decimal? OrderAmount { get; private set; }

    public decimal CommissionAmount { get; private set; }

    public decimal CashbackAmount { get; private set; }

    public decimal PlatformProfit { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime? OrderDate { get; private set; }

    public User User { get; private set; } = null!;

    public AffiliateLink? AffiliateLink { get; private set; }

    public ICollection<CommissionTransaction> CommissionTransactions { get; private set; } = [];
}
