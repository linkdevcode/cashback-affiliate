namespace Cashback.Application.Features.FinancialAudit.Common;

/// <summary>
/// Balance audit report exposed through the API.
/// </summary>
public sealed class BalanceAuditDto
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
    /// Available balance from the verification formula.
    /// </summary>
    public decimal AvailableBalance { get; init; }

    /// <summary>
    /// Balance available for new withdrawal requests.
    /// </summary>
    public decimal OperationalAvailableBalance { get; init; }

    /// <summary>
    /// Indicates whether a negative balance was detected.
    /// </summary>
    public bool HasNegativeBalance { get; init; }

    /// <summary>
    /// Indicates whether balances match the transaction ledger.
    /// </summary>
    public bool IsConsistent { get; init; }

    /// <summary>
    /// Transaction validation summary.
    /// </summary>
    public TransactionValidationDto TransactionValidation { get; init; } = new();
}
