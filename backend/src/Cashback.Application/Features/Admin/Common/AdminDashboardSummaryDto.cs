namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Aggregated platform metrics for the admin dashboard.
/// </summary>
public sealed class AdminDashboardSummaryDto
{
    /// <summary>
    /// Total number of non-deleted user accounts.
    /// </summary>
    public int TotalUsers { get; init; }

    /// <summary>
    /// Number of active user accounts.
    /// </summary>
    public int ActiveUsers { get; init; }

    /// <summary>
    /// Number of suspended user accounts.
    /// </summary>
    public int SuspendedUsers { get; init; }

    /// <summary>
    /// Total number of orders across the platform.
    /// </summary>
    public int TotalOrders { get; init; }

    /// <summary>
    /// Number of orders awaiting approval.
    /// </summary>
    public int PendingOrders { get; init; }

    /// <summary>
    /// Number of approved orders.
    /// </summary>
    public int ApprovedOrders { get; init; }

    /// <summary>
    /// Number of rejected orders.
    /// </summary>
    public int RejectedOrders { get; init; }

    /// <summary>
    /// Total number of withdrawal requests.
    /// </summary>
    public int TotalWithdrawals { get; init; }

    /// <summary>
    /// Number of withdrawal requests awaiting review.
    /// </summary>
    public int PendingWithdrawals { get; init; }

    /// <summary>
    /// Number of completed withdrawal requests.
    /// </summary>
    public int CompletedWithdrawals { get; init; }

    /// <summary>
    /// Total commission earned from approved orders.
    /// </summary>
    public decimal TotalCommission { get; init; }

    /// <summary>
    /// Total cashback paid to users from approved orders.
    /// </summary>
    public decimal TotalCashbackPaid { get; init; }

    /// <summary>
    /// Total platform revenue from approved orders.
    /// </summary>
    public decimal PlatformRevenue { get; init; }

    /// <summary>
    /// Monthly order counts for chart display.
    /// </summary>
    public IReadOnlyList<MonthlyOrderCountDto> OrdersByMonth { get; init; } = [];

    /// <summary>
    /// Monthly platform revenue totals for chart display.
    /// </summary>
    public IReadOnlyList<MonthlyRevenueDto> RevenueByMonth { get; init; } = [];

    /// <summary>
    /// Most recently registered users.
    /// </summary>
    public IReadOnlyList<AdminRecentUserDto> RecentUsers { get; init; } = [];

    /// <summary>
    /// Most recently created orders.
    /// </summary>
    public IReadOnlyList<AdminRecentOrderDto> RecentOrders { get; init; } = [];

    /// <summary>
    /// Most recently requested withdrawals.
    /// </summary>
    public IReadOnlyList<AdminRecentWithdrawalDto> RecentWithdrawals { get; init; } = [];
}
