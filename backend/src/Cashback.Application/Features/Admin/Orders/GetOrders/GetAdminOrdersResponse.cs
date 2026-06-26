using Cashback.Application.Features.Admin.Orders.Common;

namespace Cashback.Application.Features.Admin.Orders.GetOrders;

/// <summary>
/// Paginated admin order list response.
/// </summary>
public sealed class GetAdminOrdersResponse
{
    /// <summary>
    /// Orders for the current page.
    /// </summary>
    public IReadOnlyList<AdminOrderDto> Items { get; init; } = [];

    /// <summary>
    /// Total number of orders matching the query.
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
