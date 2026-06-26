namespace Cashback.Application.Interfaces;

/// <summary>
/// Balance audit report for financial consistency checks.
/// </summary>
public sealed class BalanceAuditResult
{
    /// <summary>
    /// Approved cashback earned by the user.
    /// </summary>
    public decimal ApprovedCashback { get; init; }

    /// <summary>
    /// Total amount of pending withdrawal requests.
    /// </summary>
    public decimal PendingWithdrawals { get; init; }

    /// <summary>
    /// Total amount of completed withdrawal requests.
    /// </summary>
    public decimal CompletedWithdrawals { get; init; }

    /// <summary>
    /// Total amount of approved but not yet completed withdrawals.
    /// </summary>
    public decimal ApprovedWithdrawals { get; init; }

    /// <summary>
    /// Available balance calculated from the verification formula.
    /// </summary>
    public decimal AvailableBalance { get; init; }

    /// <summary>
    /// Balance available for new withdrawal requests after approved reservations.
    /// </summary>
    public decimal OperationalAvailableBalance { get; init; }

    /// <summary>
    /// Indicates whether the available balance is negative.
    /// </summary>
    public bool HasNegativeBalance { get; init; }

    /// <summary>
    /// Indicates whether calculated balance matches the transaction ledger.
    /// </summary>
    public bool IsConsistent { get; init; }

    /// <summary>
    /// Result of transaction chain validation.
    /// </summary>
    public TransactionValidationResult TransactionValidation { get; init; } = new();
}
