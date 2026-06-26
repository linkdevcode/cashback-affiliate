using Cashback.Domain.Enums;

namespace Cashback.Application.Features.FinancialAudit.Common;

/// <summary>
/// Financial transaction record for audit trail responses.
/// </summary>
public sealed class TransactionAuditDto
{
    /// <summary>
    /// Transaction identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Transaction type value.
    /// </summary>
    public TransactionType Type { get; init; }

    /// <summary>
    /// Human-readable transaction type name.
    /// </summary>
    public string TypeName { get; init; } = null!;

    /// <summary>
    /// Transaction amount in platform currency.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// User balance before the transaction was applied.
    /// </summary>
    public decimal BalanceBefore { get; init; }

    /// <summary>
    /// User balance after the transaction was applied.
    /// </summary>
    public decimal BalanceAfter { get; init; }

    /// <summary>
    /// Optional reference to a related entity.
    /// </summary>
    public Guid? ReferenceId { get; init; }

    /// <summary>
    /// Optional description of the transaction.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// UTC timestamp when the transaction was recorded.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
