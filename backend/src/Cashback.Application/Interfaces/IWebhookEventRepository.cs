using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for webhook events.
/// </summary>
public interface IWebhookEventRepository
{
    /// <summary>
    /// Gets a webhook event by provider and provider order identifier.
    /// </summary>
    Task<WebhookEvent?> GetByProviderAndOrderIdAsync(
        string provider,
        string providerOrderId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a webhook event by identifier for update operations.
    /// </summary>
    Task<WebhookEvent?> GetByIdForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Registers a webhook event or returns the existing record for the same provider order.
    /// </summary>
    Task<WebhookEvent> RegisterAsync(
        WebhookEvent webhookEvent,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to an existing webhook event.
    /// </summary>
    Task UpdateAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken);
}
