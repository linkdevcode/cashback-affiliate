using Cashback.Application.Interfaces;

namespace Cashback.Application.Services;

/// <summary>
/// Calculates cashback amounts from affiliate commission using configured rates.
/// </summary>
public sealed class CashbackService : ICashbackService
{
    private readonly ICashbackSettings _cashbackSettings;

    /// <summary>
    /// Initializes a new instance of the cashback service.
    /// </summary>
    public CashbackService(ICashbackSettings cashbackSettings)
    {
        _cashbackSettings = cashbackSettings;
    }

    /// <inheritdoc/>
    public CashbackCalculationResult CalculateCashback(decimal commissionAmount)
    {
        var cashbackRate = _cashbackSettings.CashbackPercentage / 100m;
        var cashbackAmount = Math.Round(
            commissionAmount * cashbackRate,
            2,
            MidpointRounding.AwayFromZero);
        var platformRevenue = commissionAmount - cashbackAmount;

        return new CashbackCalculationResult(cashbackAmount, platformRevenue);
    }
}
