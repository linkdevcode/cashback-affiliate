using Cashback.Application.Features.Orders.Common;

namespace Cashback.Application.Features.Orders.GetOrders;

/// <summary>
/// Paginated order list response.
/// </summary>
public sealed class GetOrdersResponse
{
    /// <summary>
    /// Orders for the current page.
    /// </summary>
    public IReadOnlyList<OrderDto> Items { get; init; } = [];

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
