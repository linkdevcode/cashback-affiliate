using Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;
using Cashback.Application.Interfaces;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Coordinates webhook payload processing after security and idempotency checks.
/// </summary>
public interface IWebhookProcessingService
{
    /// <summary>
    /// Processes an Accesstrade webhook payload into an order record.
    /// </summary>
    Task<OrderSyncResponse> ProcessAccesstradeAsync(
        AccesstradeWebhookRequest payload,
        Guid webhookEventId,
        CancellationToken cancellationToken);
}
