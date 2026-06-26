namespace Cashback.Application.Interfaces;

/// <summary>
/// Financial balance calculations and audit trail validation.
/// </summary>
public interface IBalanceService
{
    /// <summary>
    /// Gets the available balance using approved cashback minus pending and completed withdrawals.
    /// </summary>
    Task<decimal> GetAvailableBalanceAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the balance available for new withdrawal requests.
    /// </summary>
    Task<decimal> GetOperationalAvailableBalanceAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Validates the integrity of a user's financial transaction chain.
    /// </summary>
    Task<TransactionValidationResult> ValidateTransactionsAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Builds a full balance audit report for a user.
    /// </summary>
    Task<BalanceAuditResult> GetBalanceAuditAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
