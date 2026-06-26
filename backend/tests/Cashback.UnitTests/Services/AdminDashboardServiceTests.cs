using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Moq;
using Xunit;

namespace Cashback.UnitTests.Services;

/// <summary>
/// Unit tests for admin dashboard summary calculations.
/// </summary>
public sealed class AdminDashboardServiceTests
{
    /// <summary>
    /// Verifies admin dashboard metrics are aggregated from repository statistics.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithPlatformActivity_ReturnsAggregatedStatistics()
    {
        var userRepository = CreateUserRepositoryMock();
        var orderRepository = CreateOrderRepositoryMock();
        var withdrawalRepository = CreateWithdrawalRepositoryMock();

        var service = CreateService(userRepository, orderRepository, withdrawalRepository);

        var result = await service.GetDashboardSummaryAsync(CancellationToken.None);

        Assert.Equal(120, result.TotalUsers);
        Assert.Equal(100, result.ActiveUsers);
        Assert.Equal(15, result.SuspendedUsers);
        Assert.Equal(500, result.TotalOrders);
        Assert.Equal(80, result.PendingOrders);
        Assert.Equal(350, result.ApprovedOrders);
        Assert.Equal(70, result.RejectedOrders);
        Assert.Equal(45, result.TotalWithdrawals);
        Assert.Equal(12, result.PendingWithdrawals);
        Assert.Equal(28, result.CompletedWithdrawals);
        Assert.Equal(10_000_000m, result.TotalCommission);
        Assert.Equal(8_000_000m, result.TotalCashbackPaid);
        Assert.Equal(2_000_000m, result.PlatformRevenue);
        Assert.Equal(6, result.OrdersByMonth.Count);
        Assert.Equal(6, result.RevenueByMonth.Count);
        Assert.Empty(result.RecentUsers);
        Assert.Empty(result.RecentOrders);
        Assert.Empty(result.RecentWithdrawals);
    }

    /// <summary>
    /// Verifies an empty platform returns zero dashboard totals.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_WithNoActivity_ReturnsZeroTotals()
    {
        var userRepository = CreateUserRepositoryMock(
            new AdminUserStatistics(0, 0, 0));
        var orderRepository = CreateOrderRepositoryMock(
            new AdminOrderStatistics(0, 0, 0, 0),
            new AdminRevenueStatistics(0m, 0m, 0m));
        var withdrawalRepository = CreateWithdrawalRepositoryMock(
            new AdminWithdrawalStatistics(0, 0, 0));

        var service = CreateService(userRepository, orderRepository, withdrawalRepository);

        var result = await service.GetDashboardSummaryAsync(CancellationToken.None);

        Assert.Equal(0, result.TotalUsers);
        Assert.Equal(0, result.TotalOrders);
        Assert.Equal(0, result.TotalWithdrawals);
        Assert.Equal(0m, result.TotalCommission);
        Assert.Equal(0m, result.TotalCashbackPaid);
        Assert.Equal(0m, result.PlatformRevenue);
    }

    /// <summary>
    /// Verifies all repository statistics sources are queried.
    /// </summary>
    [Fact]
    public async Task GetDashboardSummaryAsync_QueriesAllStatisticsSources()
    {
        var userRepository = CreateUserRepositoryMock(
            new AdminUserStatistics(1, 1, 0));
        var orderRepository = CreateOrderRepositoryMock(
            new AdminOrderStatistics(1, 0, 1, 0),
            new AdminRevenueStatistics(100m, 80m, 20m));
        var withdrawalRepository = CreateWithdrawalRepositoryMock(
            new AdminWithdrawalStatistics(1, 0, 1));

        var service = CreateService(userRepository, orderRepository, withdrawalRepository);

        await service.GetDashboardSummaryAsync(CancellationToken.None);

        userRepository.Verify(
            repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        userRepository.Verify(
            repository => repository.GetRecentForAdminAsync(5, It.IsAny<CancellationToken>()),
            Times.Once);
        orderRepository.Verify(
            repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        orderRepository.Verify(
            repository => repository.GetAdminRevenueStatisticsAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        orderRepository.Verify(
            repository => repository.GetMonthlyOrderCountsAsync(6, It.IsAny<CancellationToken>()),
            Times.Once);
        orderRepository.Verify(
            repository => repository.GetMonthlyRevenueTotalsAsync(6, It.IsAny<CancellationToken>()),
            Times.Once);
        orderRepository.Verify(
            repository => repository.GetRecentForAdminAsync(5, It.IsAny<CancellationToken>()),
            Times.Once);
        withdrawalRepository.Verify(
            repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        withdrawalRepository.Verify(
            repository => repository.GetRecentForAdminAsync(5, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Creates an admin dashboard service with mocked dependencies.
    /// </summary>
    private static AdminDashboardService CreateService(
        Mock<IUserRepository> userRepository,
        Mock<IOrderRepository> orderRepository,
        Mock<IWithdrawalRepository> withdrawalRepository)
    {
        return new AdminDashboardService(
            userRepository.Object,
            orderRepository.Object,
            withdrawalRepository.Object);
    }

    /// <summary>
    /// Creates a mocked user repository with default admin dashboard data.
    /// </summary>
    private static Mock<IUserRepository> CreateUserRepositoryMock(
        AdminUserStatistics? statistics = null)
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(statistics ?? new AdminUserStatistics(120, 100, 15));
        userRepository
            .Setup(repository => repository.GetRecentForAdminAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        return userRepository;
    }

    /// <summary>
    /// Creates a mocked order repository with default admin dashboard data.
    /// </summary>
    private static Mock<IOrderRepository> CreateOrderRepositoryMock(
        AdminOrderStatistics? statistics = null,
        AdminRevenueStatistics? revenueStatistics = null)
    {
        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(statistics ?? new AdminOrderStatistics(500, 80, 350, 70));
        orderRepository
            .Setup(repository => repository.GetAdminRevenueStatisticsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(revenueStatistics ?? new AdminRevenueStatistics(10_000_000m, 8_000_000m, 2_000_000m));
        orderRepository
            .Setup(repository => repository.GetMonthlyOrderCountsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateMonthlyOrderCounts());
        orderRepository
            .Setup(repository => repository.GetMonthlyRevenueTotalsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateMonthlyRevenueTotals());
        orderRepository
            .Setup(repository => repository.GetRecentForAdminAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        return orderRepository;
    }

    /// <summary>
    /// Creates a mocked withdrawal repository with default admin dashboard data.
    /// </summary>
    private static Mock<IWithdrawalRepository> CreateWithdrawalRepositoryMock(
        AdminWithdrawalStatistics? statistics = null)
    {
        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetAdminStatisticsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(statistics ?? new AdminWithdrawalStatistics(45, 12, 28));
        withdrawalRepository
            .Setup(repository => repository.GetRecentForAdminAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        return withdrawalRepository;
    }

    /// <summary>
    /// Creates sample monthly order counts for chart tests.
    /// </summary>
    private static IReadOnlyList<MonthlyOrderCount> CreateMonthlyOrderCounts()
    {
        var utcNow = DateTime.UtcNow;
        var startMonth = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddMonths(-5);

        return Enumerable.Range(0, 6)
            .Select(index =>
            {
                var monthDate = startMonth.AddMonths(index);
                return new MonthlyOrderCount(monthDate.Year, monthDate.Month, index);
            })
            .ToList();
    }

    /// <summary>
    /// Creates sample monthly revenue totals for chart tests.
    /// </summary>
    private static IReadOnlyList<MonthlyRevenueTotal> CreateMonthlyRevenueTotals()
    {
        var utcNow = DateTime.UtcNow;
        var startMonth = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddMonths(-5);

        return Enumerable.Range(0, 6)
            .Select(index =>
            {
                var monthDate = startMonth.AddMonths(index);
                return new MonthlyRevenueTotal(monthDate.Year, monthDate.Month, index * 1_000m);
            })
            .ToList();
    }
}
