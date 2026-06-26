namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for withdrawal requests.
/// </summary>
public interface IWithdrawalRepository
{
    /// <summary>
    /// Gets the total amount of completed withdrawals for a user.
    /// </summary>
    Task<decimal> GetTotalCompletedAmountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
