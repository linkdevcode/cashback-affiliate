using Cashback.Application.Features.Earnings.Common;
using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Moq;
using Xunit;

namespace Cashback.UnitTests.Services;

/// <summary>
/// Unit tests for dashboard summary calculations.
/// </summary>
public sealed class DashboardServiceTests
{
    /// <summary>
    /// Verifies dashboard metrics are calculated from earnings, withdrawals, and recent orders.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithEarningsAndWithdrawals_ReturnsCalculatedTotals()
    {
        var userId = Guid.NewGuid();
        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto
            {
                PendingCashback = 50_000m,
                ApprovedCashback = 200_000m,
                RejectedCashback = 10_000m
            });

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(80_000m);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetRecentByUserIdAsync(userId, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateRecentOrders(userId, 2));
        SetupMonthlyCashback(orderRepository, userId);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(120_000m);

        var service = CreateService(earningsService, orderRepository, withdrawalRepository, balanceService);

        var result = await service.GetDashboardSummaryAsync(userId, CancellationToken.None);

        Assert.Equal(120_000m, result.AvailableBalance);
        Assert.Equal(50_000m, result.PendingCashback);
        Assert.Equal(250_000m, result.TotalCashback);
        Assert.Equal(80_000m, result.TotalWithdrawn);
        Assert.Equal(2, result.RecentOrders.Count);
    }

    /// <summary>
    /// Verifies users with no activity receive zero dashboard totals.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithNoActivity_ReturnsZeroTotals()
    {
        var userId = Guid.NewGuid();
        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto());

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0m);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetRecentByUserIdAsync(userId, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        SetupMonthlyCashback(orderRepository, userId);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0m);

        var service = CreateService(earningsService, orderRepository, withdrawalRepository, balanceService);

        var result = await service.GetDashboardSummaryAsync(userId, CancellationToken.None);

        Assert.Equal(0m, result.AvailableBalance);
        Assert.Equal(0m, result.PendingCashback);
        Assert.Equal(0m, result.TotalCashback);
        Assert.Equal(0m, result.TotalWithdrawn);
        Assert.Empty(result.RecentOrders);
    }

    /// <summary>
    /// Verifies available balance reflects approved cashback minus completed withdrawals.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithWithdrawalsExceedingApprovedCashback_ReturnsNegativeAvailableBalance()
    {
        var userId = Guid.NewGuid();
        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto
            {
                ApprovedCashback = 100_000m
            });

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(150_000m);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetRecentByUserIdAsync(userId, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        SetupMonthlyCashback(orderRepository, userId);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(-50_000m);

        var service = CreateService(earningsService, orderRepository, withdrawalRepository, balanceService);

        var result = await service.GetDashboardSummaryAsync(userId, CancellationToken.None);

        Assert.Equal(-50_000m, result.AvailableBalance);
        Assert.Equal(100_000m, result.TotalCashback);
        Assert.Equal(150_000m, result.TotalWithdrawn);
    }

    /// <summary>
    /// Verifies total cashback includes only approved and pending amounts.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithRejectedCashback_ExcludesRejectedFromTotalCashback()
    {
        var userId = Guid.NewGuid();
        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto
            {
                PendingCashback = 30_000m,
                ApprovedCashback = 70_000m,
                RejectedCashback = 20_000m
            });

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0m);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetRecentByUserIdAsync(userId, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        SetupMonthlyCashback(orderRepository, userId);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(70_000m);

        var service = CreateService(earningsService, orderRepository, withdrawalRepository, balanceService);

        var result = await service.GetDashboardSummaryAsync(userId, CancellationToken.None);

        Assert.Equal(100_000m, result.TotalCashback);
        Assert.Equal(30_000m, result.PendingCashback);
    }

    /// <summary>
    /// Verifies recent orders are mapped to DTOs for dashboard display.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithRecentOrders_MapsOrderDetails()
    {
        var userId = Guid.NewGuid();
        var order = Order.CreateFromWebhook(
            userId,
            null,
            "ORDER-500",
            100_000m,
            70_000m,
            30_000m,
            OrderStatus.Approved);

        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto());

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0m);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetRecentByUserIdAsync(userId, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync([order]);
        SetupMonthlyCashback(orderRepository, userId);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0m);

        var service = CreateService(earningsService, orderRepository, withdrawalRepository, balanceService);

        var result = await service.GetDashboardSummaryAsync(userId, CancellationToken.None);

        var recentOrder = Assert.Single(result.RecentOrders);
        Assert.Equal(order.Id, recentOrder.Id);
        Assert.Equal("ORDER-500", recentOrder.OrderCode);
        Assert.Equal(70_000m, recentOrder.CashbackAmount);
        Assert.Equal(OrderStatus.Approved, recentOrder.Status);
    }

    /// <summary>
    /// Creates a dashboard service with mocked dependencies.
    /// </summary>
    private static DashboardService CreateService(
        Mock<IEarningsService> earningsService,
        Mock<IOrderRepository> orderRepository,
        Mock<IWithdrawalRepository> withdrawalRepository,
        Mock<IBalanceService> balanceService)
    {
        return new DashboardService(
            earningsService.Object,
            orderRepository.Object,
            withdrawalRepository.Object,
            balanceService.Object);
    }

    /// <summary>
    /// Creates a list of recent orders for test setup.
    /// </summary>
    private static IReadOnlyList<Order> CreateRecentOrders(Guid userId, int count)
    {
        return Enumerable.Range(1, count)
            .Select(index => Order.CreateFromWebhook(
                userId,
                null,
                $"ORDER-{index}",
                10_000m * index,
                7_000m * index,
                3_000m * index,
                OrderStatus.Pending))
            .ToList();
    }

    /// <summary>
    /// Configures monthly cashback repository responses for dashboard tests.
    /// </summary>
    private static void SetupMonthlyCashback(Mock<IOrderRepository> orderRepository, Guid userId)
    {
        orderRepository
            .Setup(repository => repository.GetMonthlyCashbackTotalsAsync(userId, 6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new MonthlyCashbackTotal(2026, 1, 20_000m),
                new MonthlyCashbackTotal(2026, 2, 30_000m),
                new MonthlyCashbackTotal(2026, 3, 0m),
                new MonthlyCashbackTotal(2026, 4, 0m),
                new MonthlyCashbackTotal(2026, 5, 0m),
                new MonthlyCashbackTotal(2026, 6, 50_000m)
            ]);
    }
}
