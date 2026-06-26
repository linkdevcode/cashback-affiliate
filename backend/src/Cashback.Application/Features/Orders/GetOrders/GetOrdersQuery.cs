using Cashback.Application.Features.Orders.Common;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Orders.GetOrders;

/// <summary>
/// Query to retrieve paginated orders for the authenticated user.
/// </summary>
public sealed record GetOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    OrderStatus? Status = null,
    string SortBy = "createdAt",
    string SortDirection = "desc")
    : IRequest<GetOrdersResponse>;
