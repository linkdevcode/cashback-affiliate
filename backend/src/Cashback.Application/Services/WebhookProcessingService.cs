using Cashback.Application.Features.Orders.OrderApproval;
using Cashback.Application.Features.Orders.OrderPending;
using Cashback.Application.Features.Orders.OrderRejection;
using Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Services;

/// <summary>
/// Coordinates webhook payload processing after security and idempotency checks.
/// </summary>
public sealed class WebhookProcessingService : IWebhookProcessingService
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the webhook processing service.
    /// </summary>
    public WebhookProcessingService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public Task<OrderSyncResponse> ProcessAccesstradeAsync(
        AccesstradeWebhookRequest payload,
        Guid webhookEventId,
        CancellationToken cancellationToken)
    {
        return payload.Status.Trim().ToLowerInvariant() switch
        {
            "approved" => _mediator.Send(
                new OrderApprovalCommand(
                    payload.OrderId,
                    payload.Sub1,
                    payload.Commission,
                    webhookEventId),
                cancellationToken),
            "rejected" or "cancelled" => _mediator.Send(
                new OrderRejectionCommand(
                    payload.OrderId,
                    payload.Sub1,
                    payload.Commission,
                    webhookEventId),
                cancellationToken),
            "pending" => _mediator.Send(
                new OrderPendingCommand(
                    payload.OrderId,
                    payload.Sub1,
                    payload.Commission,
                    webhookEventId),
                cancellationToken),
            _ => throw new BusinessRuleException($"Unsupported webhook status: {payload.Status}")
        };
    }
}
