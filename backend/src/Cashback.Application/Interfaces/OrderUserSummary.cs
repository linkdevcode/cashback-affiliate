namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated order statistics for a user.
/// </summary>
public sealed record OrderUserSummary(
    int TotalOrders,
    int PendingOrders,
    int ApprovedOrders,
    int RejectedOrders,
    decimal TotalCommission,
    decimal TotalCashback);
