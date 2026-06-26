namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated withdrawal statistics for a user.
/// </summary>
public sealed record WithdrawalUserSummary(
    int TotalWithdrawals,
    int PendingWithdrawals,
    int ApprovedWithdrawals,
    int RejectedWithdrawals,
    int CompletedWithdrawals,
    decimal TotalWithdrawn);
