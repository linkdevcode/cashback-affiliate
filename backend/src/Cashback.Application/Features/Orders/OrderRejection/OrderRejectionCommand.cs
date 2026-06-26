using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Orders.OrderRejection;

/// <summary>
/// Command to reject an order from a webhook synchronization event.
/// </summary>
public sealed record OrderRejectionCommand(
    string NetworkOrderId,
    string Sub1,
    decimal CommissionAmount,
    Guid WebhookEventId) : IRequest<OrderSyncResponse>;
