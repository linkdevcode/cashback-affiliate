using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Orders.GetOrderSummary;

/// <summary>
/// Handles retrieval of aggregated order statistics for the authenticated user.
/// </summary>
public sealed class GetOrderSummaryHandler : IRequestHandler<GetOrderSummaryQuery, GetOrderSummaryResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IOrderRepository _orderRepository;
    private readonly IEarningsService _earningsService;

    /// <summary>
    /// Initializes a new instance of the get order summary handler.
    /// </summary>
    public GetOrderSummaryHandler(
        ICurrentUserService currentUserService,
        IOrderRepository orderRepository,
        IEarningsService earningsService)
    {
        _currentUserService = currentUserService;
        _orderRepository = orderRepository;
        _earningsService = earningsService;
    }

    /// <inheritdoc/>
    public async Task<GetOrderSummaryResponse> Handle(
        GetOrderSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var summary = await _orderRepository.GetUserSummaryAsync(userId, cancellationToken);
        var earnings = await _earningsService.GetUserEarningsAsync(userId, cancellationToken);

        return new GetOrderSummaryResponse
        {
            TotalOrders = summary.TotalOrders,
            PendingOrders = summary.PendingOrders,
            ApprovedOrders = summary.ApprovedOrders,
            RejectedOrders = summary.RejectedOrders,
            TotalCommission = summary.TotalCommission,
            TotalCashback = summary.TotalCashback,
            PendingCashback = earnings.PendingCashback,
            ApprovedCashback = earnings.ApprovedCashback,
            RejectedCashback = earnings.RejectedCashback
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
