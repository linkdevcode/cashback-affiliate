using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// Financial transaction record for balance audit trail.
/// </summary>
public class CommissionTransaction : BaseEntity
{
    /// <summary>
    /// User affected by the transaction.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Order related to the transaction.
    /// </summary>
    public Guid? OrderId { get; private set; }

    /// <summary>
    /// Transaction amount in platform currency.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Type of balance change represented by the transaction.
    /// </summary>
    public TransactionType Type { get; private set; }

    /// <summary>
    /// Optional description of the transaction.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// UTC timestamp when the transaction was recorded.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// User associated with the transaction.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Order associated with the transaction.
    /// </summary>
    public Order? Order { get; private set; }
}
