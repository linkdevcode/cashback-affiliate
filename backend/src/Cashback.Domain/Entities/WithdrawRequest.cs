using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// User request to withdraw available balance to a bank account.
/// </summary>
public class WithdrawRequest : BaseEntity
{
    /// <summary>
    /// User who submitted the withdrawal request.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Requested withdrawal amount.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Name of the destination bank.
    /// </summary>
    public string BankName { get; private set; } = null!;

    /// <summary>
    /// Destination bank account number.
    /// </summary>
    public string BankAccountNumber { get; private set; } = null!;

    /// <summary>
    /// Name of the bank account holder.
    /// </summary>
    public string BankAccountName { get; private set; } = null!;

    /// <summary>
    /// Current processing status of the withdrawal.
    /// </summary>
    public WithdrawalStatus Status { get; private set; }

    /// <summary>
    /// Optional note attached to the request.
    /// </summary>
    public string? Note { get; private set; }

    /// <summary>
    /// UTC timestamp when the withdrawal was requested.
    /// </summary>
    public DateTime RequestedAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the withdrawal was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; private set; }

    /// <summary>
    /// User associated with the withdrawal request.
    /// </summary>
    public User User { get; private set; } = null!;
}
