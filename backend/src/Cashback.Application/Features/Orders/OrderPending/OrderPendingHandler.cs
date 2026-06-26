using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Orders.OrderPending;

/// <summary>
/// Synchronizes a pending order from a webhook event.
/// </summary>
public sealed class OrderPendingHandler : IRequestHandler<OrderPendingCommand, OrderSyncResponse>
{
    private readonly IOrderSynchronizationService _orderSynchronizationService;
    private readonly IAuditLogService _auditLogService;
    private readonly IWebhookUserResolver _userResolver;
    private readonly ILogger<OrderPendingHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the order pending handler.
    /// </summary>
    public OrderPendingHandler(
        IOrderSynchronizationService orderSynchronizationService,
        IAuditLogService auditLogService,
        IWebhookUserResolver userResolver,
        ILogger<OrderPendingHandler> logger)
    {
        _orderSynchronizationService = orderSynchronizationService;
        _auditLogService = auditLogService;
        _userResolver = userResolver;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderSyncResponse> Handle(
        OrderPendingCommand request,
        CancellationToken cancellationToken)
    {
        var syncResult = await _orderSynchronizationService.SynchronizeAsync(
            new OrderSyncRequest
            {
                NetworkOrderId = request.NetworkOrderId,
                Sub1 = request.Sub1,
                CommissionAmount = request.CommissionAmount,
                TargetStatus = OrderStatus.Pending,
                WebhookEventId = request.WebhookEventId
            },
            cancellationToken);

        var userResolution = await _userResolver.ResolveBySub1Async(request.Sub1, cancellationToken);
        if (userResolution is not null)
        {
            var auditAction = syncResult.WasCreated
                ? AuditAction.OrderCreated
                : AuditAction.OrderUpdated;

            await _auditLogService.LogOrderSynchronizationAsync(
                userResolution.UserId,
                syncResult.OrderId,
                auditAction,
                syncResult.PreviousStatus,
                OrderStatus.Pending,
                request.WebhookEventId,
                cancellationToken);
        }

        _logger.LogInformation(
            "Order {OrderId} synchronized to pending from webhook event {WebhookEventId}",
            syncResult.OrderId,
            request.WebhookEventId);

        return syncResult;
    }
}
