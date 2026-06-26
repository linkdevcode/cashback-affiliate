using Cashback.Application.Features.Admin.Withdrawals.Common;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawals;

/// <summary>
/// Paginated admin withdrawal list response.
/// </summary>
public sealed class GetAdminWithdrawalsResponse
{
    /// <summary>
    /// Withdrawals for the current page.
    /// </summary>
    public IReadOnlyList<AdminWithdrawalDto> Items { get; init; } = [];

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
