using Cashback.Application.Features.Dashboard.Common;
using Cashback.Application.Features.Orders.Common;
using Cashback.Application.Interfaces;

namespace Cashback.Application.Services;

/// <summary>
/// Aggregates dashboard metrics from earnings, orders, and withdrawals.
/// </summary>
public sealed class DashboardService : IDashboardService
{
    private const int RecentOrderCount = 5;
    private const int MonthlyCashbackCount = 6;

    private readonly IEarningsService _earningsService;
    private readonly IOrderRepository _orderRepository;
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the dashboard service.
    /// </summary>
    public DashboardService(
        IEarningsService earningsService,
        IOrderRepository orderRepository,
        IWithdrawalRepository withdrawalRepository)
    {
        _earningsService = earningsService;
        _orderRepository = orderRepository;
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var earningsTask = _earningsService.GetUserEarningsAsync(userId, cancellationToken);
        var totalWithdrawnTask = _withdrawalRepository.GetTotalCompletedAmountByUserIdAsync(
            userId,
            cancellationToken);
        var recentOrdersTask = _orderRepository.GetRecentByUserIdAsync(
            userId,
            RecentOrderCount,
            cancellationToken);
        var cashbackByMonthTask = _orderRepository.GetMonthlyCashbackTotalsAsync(
            userId,
            MonthlyCashbackCount,
            cancellationToken);

        await Task.WhenAll(earningsTask, totalWithdrawnTask, recentOrdersTask, cashbackByMonthTask);

        var earnings = await earningsTask;
        var totalWithdrawn = await totalWithdrawnTask;
        var recentOrders = await recentOrdersTask;
        var cashbackByMonth = await cashbackByMonthTask;

        var availableBalance = earnings.ApprovedCashback - totalWithdrawn;
        var totalCashback = earnings.ApprovedCashback + earnings.PendingCashback;

        return new DashboardSummaryDto
        {
            AvailableBalance = availableBalance,
            PendingCashback = earnings.PendingCashback,
            TotalCashback = totalCashback,
            TotalWithdrawn = totalWithdrawn,
            RecentOrders = recentOrders.Select(OrderMapper.ToDto).ToList(),
            CashbackByMonth = cashbackByMonth
                .Select(item => new MonthlyCashbackDto
                {
                    Year = item.Year,
                    Month = item.Month,
                    CashbackAmount = item.CashbackAmount
                })
                .ToList()
        };
    }
}
