using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Services;

/// <summary>
/// Coordinates webhook duplicate detection and processed event tracking.
/// </summary>
public sealed class WebhookIdempotencyService : IWebhookIdempotencyService
{
    private readonly IWebhookEventRepository _webhookEventRepository;
    private readonly ILogger<WebhookIdempotencyService> _logger;

    /// <summary>
    /// Initializes a new instance of the webhook idempotency service.
    /// </summary>
    public WebhookIdempotencyService(
        IWebhookEventRepository webhookEventRepository,
        ILogger<WebhookIdempotencyService> logger)
    {
        _webhookEventRepository = webhookEventRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<WebhookIdempotencyResult> RegisterOrGetDuplicateAsync(
        WebhookIdempotencyRequest request,
        CancellationToken cancellationToken)
    {
        var webhookEvent = WebhookEvent.Create(
            request.Provider,
            request.ProviderOrderId,
            request.Payload,
            request.EventId);

        var registeredEvent = await _webhookEventRepository.RegisterAsync(
            webhookEvent,
            cancellationToken);

        if (ShouldSkipAsDuplicate(registeredEvent))
        {
            _logger.LogInformation(
                "Duplicate webhook ignored for provider {Provider} and order {ProviderOrderId}",
                registeredEvent.Provider,
                registeredEvent.ProviderOrderId);

            return WebhookIdempotencyResult.Duplicate(registeredEvent.Id);
        }

        if (registeredEvent.Status == WebhookEventStatus.Received
            || registeredEvent.Status == WebhookEventStatus.Failed)
        {
            registeredEvent.MarkAsProcessing();
            await _webhookEventRepository.UpdateAsync(registeredEvent, cancellationToken);
        }

        return WebhookIdempotencyResult.Accepted(registeredEvent.Id);
    }

    /// <inheritdoc/>
    public async Task MarkAsProcessedAsync(Guid webhookEventId, CancellationToken cancellationToken)
    {
        var webhookEvent = await _webhookEventRepository.GetByIdForUpdateAsync(
            webhookEventId,
            cancellationToken);

        if (webhookEvent is null)
        {
            _logger.LogWarning(
                "Webhook event {WebhookEventId} was not found when marking as processed",
                webhookEventId);
            return;
        }

        if (webhookEvent.IsAlreadyProcessed)
        {
            return;
        }

        webhookEvent.MarkAsProcessed();
        await _webhookEventRepository.UpdateAsync(webhookEvent, cancellationToken);

        _logger.LogInformation(
            "Webhook event {WebhookEventId} marked as processed for provider {Provider}",
            webhookEvent.Id,
            webhookEvent.Provider);
    }

    /// <inheritdoc/>
    public async Task MarkAsFailedAsync(
        Guid webhookEventId,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        var webhookEvent = await _webhookEventRepository.GetByIdForUpdateAsync(
            webhookEventId,
            cancellationToken);

        if (webhookEvent is null)
        {
            _logger.LogWarning(
                "Webhook event {WebhookEventId} was not found when marking as failed",
                webhookEventId);
            return;
        }

        webhookEvent.MarkAsFailed(errorMessage);
        await _webhookEventRepository.UpdateAsync(webhookEvent, cancellationToken);

        _logger.LogWarning(
            "Webhook event {WebhookEventId} marked as failed: {ErrorMessage}",
            webhookEvent.Id,
            errorMessage);
    }

    /// <summary>
    /// Determines whether an existing webhook event should be treated as a duplicate.
    /// </summary>
    private static bool ShouldSkipAsDuplicate(WebhookEvent webhookEvent)
    {
        return webhookEvent.IsAlreadyProcessed
            || webhookEvent.Status == WebhookEventStatus.Processing;
    }
}
