using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Moq;
using Xunit;

namespace Cashback.UnitTests.Services;

/// <summary>
/// Unit tests for cashback calculation logic.
/// </summary>
public sealed class CashbackServiceTests
{
    /// <summary>
    /// Verifies default 80% cashback rate calculation.
    /// </summary>
    [Fact]
    public void CalculateCashback_DefaultRate_ReturnsExpectedAmounts()
    {
        var settings = CreateSettings(80m, 20m);
        var service = new CashbackService(settings);

        var result = service.CalculateCashback(100_000m);

        Assert.Equal(80_000m, result.CashbackAmount);
        Assert.Equal(20_000m, result.PlatformRevenue);
    }

    /// <summary>
    /// Verifies custom cashback percentage is applied correctly.
    /// </summary>
    [Fact]
    public void CalculateCashback_CustomRate_ReturnsExpectedAmounts()
    {
        var settings = CreateSettings(70m, 30m);
        var service = new CashbackService(settings);

        var result = service.CalculateCashback(100_000m);

        Assert.Equal(70_000m, result.CashbackAmount);
        Assert.Equal(30_000m, result.PlatformRevenue);
    }

    /// <summary>
    /// Verifies zero commission returns zero cashback and platform revenue.
    /// </summary>
    [Fact]
    public void CalculateCashback_ZeroCommission_ReturnsZeroAmounts()
    {
        var settings = CreateSettings(80m, 20m);
        var service = new CashbackService(settings);

        var result = service.CalculateCashback(0m);

        Assert.Equal(0m, result.CashbackAmount);
        Assert.Equal(0m, result.PlatformRevenue);
    }

    /// <summary>
    /// Verifies fractional commission amounts are rounded to two decimal places.
    /// </summary>
    [Fact]
    public void CalculateCashback_FractionalCommission_RoundsToTwoDecimals()
    {
        var settings = CreateSettings(80m, 20m);
        var service = new CashbackService(settings);

        var result = service.CalculateCashback(33.33m);

        Assert.Equal(26.66m, result.CashbackAmount);
        Assert.Equal(6.67m, result.PlatformRevenue);
    }

    /// <summary>
    /// Verifies cashback and platform revenue sum to commission amount.
    /// </summary>
    [Fact]
    public void CalculateCashback_Result_SumsToCommissionAmount()
    {
        var settings = CreateSettings(80m, 20m);
        var service = new CashbackService(settings);
        const decimal commissionAmount = 123_456.78m;

        var result = service.CalculateCashback(commissionAmount);

        Assert.Equal(commissionAmount, result.CashbackAmount + result.PlatformRevenue);
    }

    private static ICashbackSettings CreateSettings(
        decimal cashbackPercentage,
        decimal platformCommissionPercentage)
    {
        var mock = new Mock<ICashbackSettings>();
        mock.Setup(settings => settings.CashbackPercentage).Returns(cashbackPercentage);
        mock.Setup(settings => settings.PlatformCommissionPercentage).Returns(platformCommissionPercentage);
        return mock.Object;
    }
}
