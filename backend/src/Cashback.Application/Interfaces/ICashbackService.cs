namespace Cashback.Application.Interfaces;

/// <summary>
/// Calculates cashback amounts from affiliate commission.
/// </summary>
public interface ICashbackService
{
    /// <summary>
    /// Calculates user cashback and platform revenue from commission amount.
    /// </summary>
    CashbackCalculationResult CalculateCashback(decimal commissionAmount);
}
