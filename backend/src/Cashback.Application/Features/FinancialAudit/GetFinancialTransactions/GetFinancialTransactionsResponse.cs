using Cashback.Application.Features.FinancialAudit.Common;

namespace Cashback.Application.Features.FinancialAudit.GetFinancialTransactions;

/// <summary>
/// Paginated financial transaction audit response.
/// </summary>
public sealed class GetFinancialTransactionsResponse
{
    /// <summary>
    /// Transactions for the current page.
    /// </summary>
    public IReadOnlyList<TransactionAuditDto> Items { get; init; } = [];

    /// <summary>
    /// Total number of transactions matching the query.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; }
}
