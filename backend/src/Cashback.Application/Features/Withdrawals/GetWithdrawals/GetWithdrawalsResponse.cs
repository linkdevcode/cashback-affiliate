using Cashback.Application.Features.Withdrawals.Common;

namespace Cashback.Application.Features.Withdrawals.GetWithdrawals;

/// <summary>
/// Paginated withdrawal list response.
/// </summary>
public sealed class GetWithdrawalsResponse
{
    /// <summary>
    /// Withdrawals for the current page.
    /// </summary>
    public IReadOnlyList<WithdrawalDto> Items { get; init; } = [];

    /// <summary>
    /// Total number of withdrawals matching the query.
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
