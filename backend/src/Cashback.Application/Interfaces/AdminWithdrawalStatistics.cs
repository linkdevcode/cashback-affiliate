namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated withdrawal statistics for the admin dashboard.
/// </summary>
public sealed record AdminWithdrawalStatistics(
    int TotalWithdrawals,
    int PendingWithdrawals,
    int CompletedWithdrawals);
