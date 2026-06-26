namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// Withdrawal summary for admin user detail responses.
/// </summary>
public sealed class AdminUserWithdrawalSummaryDto
{
    /// <summary>
    /// Total number of withdrawal requests.
    /// </summary>
    public int TotalWithdrawals { get; init; }

    /// <summary>
    /// Number of pending withdrawal requests.
    /// </summary>
    public int PendingWithdrawals { get; init; }

    /// <summary>
    /// Number of approved withdrawal requests.
    /// </summary>
    public int ApprovedWithdrawals { get; init; }

    /// <summary>
    /// Number of rejected withdrawal requests.
    /// </summary>
    public int RejectedWithdrawals { get; init; }

    /// <summary>
    /// Number of completed withdrawal requests.
    /// </summary>
    public int CompletedWithdrawals { get; init; }

    /// <summary>
    /// Total amount withdrawn through completed requests.
    /// </summary>
    public decimal TotalWithdrawn { get; init; }
}
