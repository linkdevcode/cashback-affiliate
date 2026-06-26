using Cashback.Application.Features.Orders.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Orders.GetOrders;

/// <summary>
/// Handles retrieval of paginated orders for the authenticated user.
/// </summary>
public sealed class GetOrdersHandler : IRequestHandler<GetOrdersQuery, GetOrdersResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the get orders handler.
    /// </summary>
    public GetOrdersHandler(
        ICurrentUserService currentUserService,
        IOrderRepository orderRepository)
    {
        _currentUserService = currentUserService;
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<GetOrdersResponse> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var (items, totalCount) = await _orderRepository.GetPagedByUserIdAsync(
            userId,
            request.Page,
            request.PageSize,
            request.Status,
            request.SortBy,
            request.SortDirection,
            cancellationToken);

        return new GetOrdersResponse
        {
            Items = items.Select(OrderMapper.ToDto).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
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
