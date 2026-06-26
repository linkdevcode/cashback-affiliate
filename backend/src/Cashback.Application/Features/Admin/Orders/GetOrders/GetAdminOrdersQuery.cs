using Cashback.Application.Features.Admin.Orders.Common;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Admin.Orders.GetOrders;

/// <summary>
/// Query to retrieve paginated orders for admin management.
/// </summary>
public sealed record GetAdminOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    string? OrderId = null,
    string? User = null,
    OrderStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null)
    : IRequest<GetAdminOrdersResponse>;
