using Cashback.Application.Features.Dashboard.Common;
using Cashback.Application.Features.Orders.Common;

namespace Cashback.Application.Features.Dashboard.GetDashboardSummary;

/// <summary>
/// Dashboard summary response for the authenticated user.
/// </summary>
public sealed class GetDashboardSummaryResponse
{
    /// <summary>
    /// Amount available for withdrawal after completed payouts.
    /// </summary>
    public decimal AvailableBalance { get; init; }

    /// <summary>
    /// Cashback from orders awaiting approval.
    /// </summary>
    public decimal PendingCashback { get; init; }

    /// <summary>
    /// Total cashback from approved and pending orders.
    /// </summary>
    public decimal TotalCashback { get; init; }

    /// <summary>
    /// Total amount withdrawn through completed requests.
    /// </summary>
    public decimal TotalWithdrawn { get; init; }

    /// <summary>
    /// Latest orders for the user.
    /// </summary>
    public IReadOnlyList<OrderDto> RecentOrders { get; init; } = [];

    /// <summary>
    /// Monthly cashback totals for chart display.
    /// </summary>
    public IReadOnlyList<MonthlyCashbackDto> CashbackByMonth { get; init; } = [];
}
