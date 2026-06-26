using Cashback.Application.Features.Admin.Orders.Common;
using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Admin.Orders.GetOrders;

/// <summary>
/// Handles retrieval of paginated orders for admin management.
/// </summary>
public sealed class GetAdminOrdersHandler : IRequestHandler<GetAdminOrdersQuery, GetAdminOrdersResponse>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the get admin orders handler.
    /// </summary>
    public GetAdminOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<GetAdminOrdersResponse> Handle(
        GetAdminOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _orderRepository.GetPagedForAdminAsync(
            request.Page,
            request.PageSize,
            request.OrderId,
            request.User,
            request.Status,
            request.FromDate,
            request.ToDate,
            cancellationToken);

        return new GetAdminOrdersResponse
        {
            Items = items.Select(AdminOrderMapper.ToListItem).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
