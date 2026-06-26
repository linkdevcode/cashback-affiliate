using Cashback.Application.Features.Admin.Orders.Common;
using MediatR;

namespace Cashback.Application.Features.Admin.Orders.GetOrderDetail;

/// <summary>
/// Query to retrieve detailed order information for admin management.
/// </summary>
public sealed record GetAdminOrderDetailQuery(Guid OrderId) : IRequest<AdminOrderDetailDto>;
