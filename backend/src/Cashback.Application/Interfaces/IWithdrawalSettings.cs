namespace Cashback.Application.Interfaces;

/// <summary>
/// Business configuration for withdrawal requests.
/// </summary>
public interface IWithdrawalSettings
{
    /// <summary>
    /// Minimum amount allowed for a withdrawal request.
    /// </summary>
    decimal MinimumWithdrawalAmount { get; }
}
