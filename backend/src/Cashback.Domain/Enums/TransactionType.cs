namespace Cashback.Domain.Enums;

/// <summary>
/// Type of financial balance transaction.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Cashback credited after order approval.
    /// </summary>
    CashbackEarned = 1,

    /// <summary>
    /// Balance reserved when a withdrawal is requested.
    /// </summary>
    WithdrawalRequested = 2,

    /// <summary>
    /// Withdrawal approved by admin.
    /// </summary>
    WithdrawalApproved = 3,

    /// <summary>
    /// Withdrawal rejected and balance restored.
    /// </summary>
    WithdrawalRejected = 4,

    /// <summary>
    /// Withdrawal completed and paid out.
    /// </summary>
    WithdrawalCompleted = 5
}
