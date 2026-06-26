namespace Cashback.Application.Features.FinancialAudit.Common;

/// <summary>
/// Transaction validation summary for audit responses.
/// </summary>
public sealed class TransactionValidationDto
{
    /// <summary>
    /// Indicates whether all transaction records passed validation.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// Indicates whether any transaction produced a negative balance.
    /// </summary>
    public bool HasNegativeBalance { get; init; }

    /// <summary>
    /// Balance recorded after the latest transaction.
    /// </summary>
    public decimal? LedgerBalance { get; init; }

    /// <summary>
    /// Validation issues detected in the transaction chain.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = [];
}
