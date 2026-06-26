using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Moq;
using Xunit;

namespace Cashback.UnitTests.Services;

/// <summary>
/// Unit tests for earnings summary calculations.
/// </summary>
public sealed class EarningsServiceTests
{
    /// <summary>
    /// Verifies earnings are mapped from repository totals by status.
    /// </summary>
    [Fact]
    public async Task GetUserEarningsAsync_WithOrders_ReturnsStatusTotals()
    {
        var userId = Guid.NewGuid();
        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetEarningsByStatusAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsByStatus(50_000m, 120_000m, 10_000m));

        var service = new EarningsService(orderRepository.Object);

        var result = await service.GetUserEarningsAsync(userId, CancellationToken.None);

        Assert.Equal(50_000m, result.PendingCashback);
        Assert.Equal(120_000m, result.ApprovedCashback);
        Assert.Equal(10_000m, result.RejectedCashback);
    }

    /// <summary>
    /// Verifies users with no orders receive zero earnings totals.
    /// </summary>
    [Fact]
    public async Task GetUserEarningsAsync_WithNoOrders_ReturnsZeroTotals()
    {
        var userId = Guid.NewGuid();
        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetEarningsByStatusAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsByStatus(0m, 0m, 0m));

        var service = new EarningsService(orderRepository.Object);

        var result = await service.GetUserEarningsAsync(userId, CancellationToken.None);

        Assert.Equal(0m, result.PendingCashback);
        Assert.Equal(0m, result.ApprovedCashback);
        Assert.Equal(0m, result.RejectedCashback);
    }

    /// <summary>
    /// Verifies only pending cashback is returned when user has pending orders only.
    /// </summary>
    [Fact]
    public async Task GetUserEarningsAsync_WithPendingOrdersOnly_ReturnsPendingCashback()
    {
        var userId = Guid.NewGuid();
        var orderRepository = new Mock<IOrderRepository>();
        orderRepository
            .Setup(repository => repository.GetEarningsByStatusAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsByStatus(75_000m, 0m, 0m));

        var service = new EarningsService(orderRepository.Object);

        var result = await service.GetUserEarningsAsync(userId, CancellationToken.None);

        Assert.Equal(75_000m, result.PendingCashback);
        Assert.Equal(0m, result.ApprovedCashback);
        Assert.Equal(0m, result.RejectedCashback);
    }
}
