using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// Financial transaction record for balance audit trail.
/// </summary>
public class Transaction : BaseEntity
{
    /// <summary>
    /// User affected by the transaction.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Type of balance change represented by the transaction.
    /// </summary>
    public TransactionType Type { get; private set; }

    /// <summary>
    /// Transaction amount in platform currency.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// User balance before the transaction was applied.
    /// </summary>
    public decimal BalanceBefore { get; private set; }

    /// <summary>
    /// User balance after the transaction was applied.
    /// </summary>
    public decimal BalanceAfter { get; private set; }

    /// <summary>
    /// Optional reference to a related entity such as an order or withdrawal.
    /// </summary>
    public Guid? ReferenceId { get; private set; }

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
    /// Required by Entity Framework.
    /// </summary>
    private Transaction()
    {
    }

    /// <summary>
    /// Creates a new financial transaction record.
    /// </summary>
    public static Transaction Create(
        Guid userId,
        TransactionType type,
        decimal amount,
        decimal balanceBefore,
        decimal balanceAfter,
        Guid? referenceId = null,
        string? description = null)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Amount = amount,
            BalanceBefore = balanceBefore,
            BalanceAfter = balanceAfter,
            ReferenceId = referenceId,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }
}
