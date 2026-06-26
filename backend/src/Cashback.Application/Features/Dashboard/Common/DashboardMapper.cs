using Cashback.Application.Features.Dashboard.Common;
using Cashback.Application.Features.Dashboard.GetDashboardSummary;

namespace Cashback.Application.Features.Dashboard.Common;

/// <summary>
/// Maps dashboard DTOs to API responses.
/// </summary>
public static class DashboardMapper
{
    /// <summary>
    /// Maps a dashboard summary DTO to an API response.
    /// </summary>
    public static GetDashboardSummaryResponse ToResponse(DashboardSummaryDto summary)
    {
        return new GetDashboardSummaryResponse
        {
            AvailableBalance = summary.AvailableBalance,
            PendingCashback = summary.PendingCashback,
            TotalCashback = summary.TotalCashback,
            TotalWithdrawn = summary.TotalWithdrawn,
            RecentOrders = summary.RecentOrders,
            CashbackByMonth = summary.CashbackByMonth
        };
    }
}
