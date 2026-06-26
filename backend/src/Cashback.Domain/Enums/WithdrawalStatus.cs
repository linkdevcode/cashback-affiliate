namespace Cashback.Domain.Enums;

/// <summary>
/// Withdrawal request processing status.
/// </summary>
public enum WithdrawalStatus
{
    /// <summary>
    /// Withdrawal is awaiting admin review.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Withdrawal has been approved for payment.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Withdrawal was rejected by an administrator.
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// Withdrawal has been completed and paid out.
    /// </summary>
    Completed = 4
}
