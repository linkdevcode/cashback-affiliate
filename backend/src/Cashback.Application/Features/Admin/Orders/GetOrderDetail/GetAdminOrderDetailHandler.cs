using Cashback.Application.Features.Admin.Orders.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Admin.Orders.GetOrderDetail;

/// <summary>
/// Handles retrieval of detailed order information for admin management.
/// </summary>
public sealed class GetAdminOrderDetailHandler : IRequestHandler<GetAdminOrderDetailQuery, AdminOrderDetailDto>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the get admin order detail handler.
    /// </summary>
    public GetAdminOrderDetailHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<AdminOrderDetailDto> Handle(
        GetAdminOrderDetailQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdForAdminAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException("Order not found.");
        }

        return AdminOrderMapper.ToDetail(order);
    }
}
