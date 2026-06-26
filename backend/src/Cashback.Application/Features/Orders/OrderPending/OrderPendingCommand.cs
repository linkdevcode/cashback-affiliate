using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Orders.OrderPending;

/// <summary>
/// Command to synchronize a pending order from a webhook event.
/// </summary>
public sealed record OrderPendingCommand(
    string NetworkOrderId,
    string Sub1,
    decimal CommissionAmount,
    Guid WebhookEventId) : IRequest<OrderSyncResponse>;
