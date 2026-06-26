namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated revenue statistics for the admin dashboard.
/// </summary>
public sealed record AdminRevenueStatistics(
    decimal TotalCommission,
    decimal TotalCashbackPaid,
    decimal PlatformRevenue);
