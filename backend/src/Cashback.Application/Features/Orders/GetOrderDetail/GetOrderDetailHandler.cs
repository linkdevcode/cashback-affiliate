using Cashback.Application.Features.Orders.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Orders.GetOrderDetail;

/// <summary>
/// Handles retrieval of a single order with ownership validation.
/// </summary>
public sealed class GetOrderDetailHandler : IRequestHandler<GetOrderDetailQuery, OrderDetailDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the get order detail handler.
    /// </summary>
    public GetOrderDetailHandler(
        ICurrentUserService currentUserService,
        IOrderRepository orderRepository)
    {
        _currentUserService = currentUserService;
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailDto> Handle(
        GetOrderDetailQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var order = await _orderRepository.GetByIdForUserAsync(
            request.Id,
            userId,
            cancellationToken);

        if (order is null)
        {
            throw new NotFoundException("Order not found.");
        }

        return OrderMapper.ToDetailDto(order);
    }

    /// <summary>
    /// Resolves the authenticated user identifier from the request context.
    /// </summary>
    private Guid GetAuthenticatedUserId()
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        return _currentUserService.UserId.Value;
    }
}
