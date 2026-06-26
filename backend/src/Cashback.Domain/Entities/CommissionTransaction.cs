using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class CommissionTransaction : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid? OrderId { get; private set; }

    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public Order? Order { get; private set; }
}
