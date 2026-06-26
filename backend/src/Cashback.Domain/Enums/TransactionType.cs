namespace Cashback.Domain.Enums;

/// <summary>
/// Type of commission balance transaction.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Cashback detected but not yet approved.
    /// </summary>
    CashbackPending = 1,

    /// <summary>
    /// Approved cashback credited to balance.
    /// </summary>
    CashbackApproved = 2,

    /// <summary>
    /// Previously approved cashback reversed.
    /// </summary>
    CashbackReversed = 3,

    /// <summary>
    /// Balance reserved for a withdrawal request.
    /// </summary>
    WithdrawalRequested = 4,

    /// <summary>
    /// Withdrawal completed and paid out.
    /// </summary>
    WithdrawalCompleted = 5,

    /// <summary>
    /// Manual balance adjustment.
    /// </summary>
    Adjustment = 6
}
