using Cashback.Application.Features.Orders.Common;
using MediatR;

namespace Cashback.Application.Features.Orders.GetOrderDetail;

/// <summary>
/// Query to retrieve a single order owned by the authenticated user.
/// </summary>
public sealed record GetOrderDetailQuery(Guid Id) : IRequest<OrderDetailDto>;
