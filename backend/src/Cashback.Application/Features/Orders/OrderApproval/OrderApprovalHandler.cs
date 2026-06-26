using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Orders.OrderApproval;

/// <summary>
/// Approves an order from a webhook synchronization event.
/// </summary>
public sealed class OrderApprovalHandler : IRequestHandler<OrderApprovalCommand, OrderSyncResponse>
{
    private readonly IOrderSynchronizationService _orderSynchronizationService;
    private readonly IAuditLogService _auditLogService;
    private readonly IWebhookUserResolver _userResolver;
    private readonly ILogger<OrderApprovalHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the order approval handler.
    /// </summary>
    public OrderApprovalHandler(
        IOrderSynchronizationService orderSynchronizationService,
        IAuditLogService auditLogService,
        IWebhookUserResolver userResolver,
        ILogger<OrderApprovalHandler> logger)
    {
        _orderSynchronizationService = orderSynchronizationService;
        _auditLogService = auditLogService;
        _userResolver = userResolver;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderSyncResponse> Handle(
        OrderApprovalCommand request,
        CancellationToken cancellationToken)
    {
        var syncResult = await _orderSynchronizationService.SynchronizeAsync(
            new OrderSyncRequest
            {
                NetworkOrderId = request.NetworkOrderId,
                Sub1 = request.Sub1,
                CommissionAmount = request.CommissionAmount,
                TargetStatus = OrderStatus.Approved,
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
                OrderStatus.Approved,
                request.WebhookEventId,
                cancellationToken);
        }

        _logger.LogInformation(
            "Order {OrderId} approved from webhook event {WebhookEventId}",
            syncResult.OrderId,
            request.WebhookEventId);

        return syncResult;
    }
}
