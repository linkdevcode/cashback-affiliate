using Cashback.Application.Features.Admin.Common;
using Cashback.Application.Interfaces;

namespace Cashback.Application.Services;

/// <summary>
/// Aggregates platform-wide statistics for the admin dashboard.
/// </summary>
public sealed class AdminDashboardService : IAdminDashboardService
{
    private const int MonthlyChartCount = 6;
    private const int RecentActivityCount = 5;

    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the admin dashboard service.
    /// </summary>
    public AdminDashboardService(
        IUserRepository userRepository,
        IOrderRepository orderRepository,
        IWithdrawalRepository withdrawalRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync(
        CancellationToken cancellationToken)
    {
        var userStatisticsTask = _userRepository.GetAdminStatisticsAsync(cancellationToken);
        var orderStatisticsTask = _orderRepository.GetAdminStatisticsAsync(cancellationToken);
        var withdrawalStatisticsTask = _withdrawalRepository.GetAdminStatisticsAsync(cancellationToken);
        var revenueStatisticsTask = _orderRepository.GetAdminRevenueStatisticsAsync(cancellationToken);
        var ordersByMonthTask = _orderRepository.GetMonthlyOrderCountsAsync(
            MonthlyChartCount,
            cancellationToken);
        var revenueByMonthTask = _orderRepository.GetMonthlyRevenueTotalsAsync(
            MonthlyChartCount,
            cancellationToken);
        var recentUsersTask = _userRepository.GetRecentForAdminAsync(
            RecentActivityCount,
            cancellationToken);
        var recentOrdersTask = _orderRepository.GetRecentForAdminAsync(
            RecentActivityCount,
            cancellationToken);
        var recentWithdrawalsTask = _withdrawalRepository.GetRecentForAdminAsync(
            RecentActivityCount,
            cancellationToken);

        await Task.WhenAll(
            userStatisticsTask,
            orderStatisticsTask,
            withdrawalStatisticsTask,
            revenueStatisticsTask,
            ordersByMonthTask,
            revenueByMonthTask,
            recentUsersTask,
            recentOrdersTask,
            recentWithdrawalsTask);

        var userStatistics = await userStatisticsTask;
        var orderStatistics = await orderStatisticsTask;
        var withdrawalStatistics = await withdrawalStatisticsTask;
        var revenueStatistics = await revenueStatisticsTask;
        var ordersByMonth = await ordersByMonthTask;
        var revenueByMonth = await revenueByMonthTask;
        var recentUsers = await recentUsersTask;
        var recentOrders = await recentOrdersTask;
        var recentWithdrawals = await recentWithdrawalsTask;

        return new AdminDashboardSummaryDto
        {
            TotalUsers = userStatistics.TotalUsers,
            ActiveUsers = userStatistics.ActiveUsers,
            SuspendedUsers = userStatistics.SuspendedUsers,
            TotalOrders = orderStatistics.TotalOrders,
            PendingOrders = orderStatistics.PendingOrders,
            ApprovedOrders = orderStatistics.ApprovedOrders,
            RejectedOrders = orderStatistics.RejectedOrders,
            TotalWithdrawals = withdrawalStatistics.TotalWithdrawals,
            PendingWithdrawals = withdrawalStatistics.PendingWithdrawals,
            CompletedWithdrawals = withdrawalStatistics.CompletedWithdrawals,
            TotalCommission = revenueStatistics.TotalCommission,
            TotalCashbackPaid = revenueStatistics.TotalCashbackPaid,
            PlatformRevenue = revenueStatistics.PlatformRevenue,
            OrdersByMonth = ordersByMonth
                .Select(item => new MonthlyOrderCountDto
                {
                    Year = item.Year,
                    Month = item.Month,
                    OrderCount = item.OrderCount
                })
                .ToList(),
            RevenueByMonth = revenueByMonth
                .Select(item => new MonthlyRevenueDto
                {
                    Year = item.Year,
                    Month = item.Month,
                    RevenueAmount = item.RevenueAmount
                })
                .ToList(),
            RecentUsers = recentUsers.Select(AdminDashboardMapper.ToRecentUser).ToList(),
            RecentOrders = recentOrders.Select(AdminDashboardMapper.ToRecentOrder).ToList(),
            RecentWithdrawals = recentWithdrawals
                .Select(AdminDashboardMapper.ToRecentWithdrawal)
                .ToList()
        };
    }
}
