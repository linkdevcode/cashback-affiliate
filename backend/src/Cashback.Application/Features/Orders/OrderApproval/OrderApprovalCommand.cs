using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Orders.OrderApproval;

/// <summary>
/// Command to approve an order from a webhook synchronization event.
/// </summary>
public sealed record OrderApprovalCommand(
    string NetworkOrderId,
    string Sub1,
    decimal CommissionAmount,
    Guid WebhookEventId) : IRequest<OrderSyncResponse>;
