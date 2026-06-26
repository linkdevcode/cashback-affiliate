namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated order statistics for the admin dashboard.
/// </summary>
public sealed record AdminOrderStatistics(
    int TotalOrders,
    int PendingOrders,
    int ApprovedOrders,
    int RejectedOrders);
