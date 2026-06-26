using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Services;

/// <summary>
/// Creates or updates orders from webhook synchronization events.
/// </summary>
public sealed class OrderSynchronizationService : IOrderSynchronizationService
{
    private readonly IWebhookUserResolver _userResolver;
    private readonly IOrderRepository _orderRepository;
    private readonly ICashbackService _cashbackService;
    private readonly ILogger<OrderSynchronizationService> _logger;

    /// <summary>
    /// Initializes a new instance of the order synchronization service.
    /// </summary>
    public OrderSynchronizationService(
        IWebhookUserResolver userResolver,
        IOrderRepository orderRepository,
        ICashbackService cashbackService,
        ILogger<OrderSynchronizationService> logger)
    {
        _userResolver = userResolver;
        _orderRepository = orderRepository;
        _cashbackService = cashbackService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderSyncResponse> SynchronizeAsync(
        OrderSyncRequest request,
        CancellationToken cancellationToken)
    {
        var userResolution = await _userResolver.ResolveBySub1Async(request.Sub1, cancellationToken);
        if (userResolution is null)
        {
            throw new BusinessRuleException("Unable to resolve user from webhook tracking parameter.");
        }

        var cashback = _cashbackService.CalculateCashback(request.CommissionAmount);
        var existingOrder = await _orderRepository.GetByNetworkOrderIdForUpdateAsync(
            request.NetworkOrderId,
            cancellationToken);

        if (existingOrder is not null)
        {
            if (existingOrder.UserId != userResolution.UserId)
            {
                throw new BusinessRuleException("Webhook order ownership does not match existing order.");
            }

            var previousStatus = existingOrder.Status;

            existingOrder.UpdateFromWebhook(
                request.CommissionAmount,
                cashback.CashbackAmount,
                cashback.PlatformRevenue,
                request.TargetStatus);

            await _orderRepository.UpdateAsync(existingOrder, cancellationToken);

            _logger.LogInformation(
                "Order {OrderId} synchronized to {OrderStatus} from webhook event {WebhookEventId}",
                existingOrder.Id,
                request.TargetStatus,
                request.WebhookEventId);

            return new OrderSyncResponse
            {
                OrderId = existingOrder.Id,
                Status = existingOrder.Status,
                WasUpdated = true,
                PreviousStatus = previousStatus
            };
        }

        var order = Order.CreateFromWebhook(
            userResolution.UserId,
            userResolution.AffiliateLinkId,
            request.NetworkOrderId,
            request.CommissionAmount,
            cashback.CashbackAmount,
            cashback.PlatformRevenue,
            request.TargetStatus);

        await _orderRepository.AddAsync(order, cancellationToken);

        _logger.LogInformation(
            "Order {OrderId} created with status {OrderStatus} from webhook event {WebhookEventId}",
            order.Id,
            request.TargetStatus,
            request.WebhookEventId);

        return new OrderSyncResponse
        {
            OrderId = order.Id,
            Status = order.Status,
            WasCreated = true
        };
    }
}
