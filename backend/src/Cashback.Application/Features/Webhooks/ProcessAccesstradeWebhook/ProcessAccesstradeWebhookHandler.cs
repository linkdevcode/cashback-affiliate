using Cashback.Application.Constants;
using Cashback.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;

/// <summary>
/// Handles incoming Accesstrade webhook postbacks.
/// </summary>
public sealed class ProcessAccesstradeWebhookHandler
    : IRequestHandler<ProcessAccesstradeWebhookCommand, AccesstradeWebhookResponse>
{
    private readonly IWebhookIdempotencyService _idempotencyService;
    private readonly IWebhookProcessingService _webhookProcessingService;
    private readonly ILogger<ProcessAccesstradeWebhookHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the Accesstrade webhook handler.
    /// </summary>
    public ProcessAccesstradeWebhookHandler(
        IWebhookIdempotencyService idempotencyService,
        IWebhookProcessingService webhookProcessingService,
        ILogger<ProcessAccesstradeWebhookHandler> logger)
    {
        _idempotencyService = idempotencyService;
        _webhookProcessingService = webhookProcessingService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AccesstradeWebhookResponse> Handle(
        ProcessAccesstradeWebhookCommand request,
        CancellationToken cancellationToken)
    {
        var payload = request.Payload;

        var idempotencyResult = await _idempotencyService.RegisterOrGetDuplicateAsync(
            new WebhookIdempotencyRequest
            {
                Provider = WebhookProviders.Accesstrade,
                ProviderOrderId = payload.OrderId,
                EventId = payload.OrderId,
                Payload = request.RawPayload
            },
            cancellationToken);

        if (idempotencyResult.IsDuplicate)
        {
            _logger.LogInformation(
                "Duplicate Accesstrade webhook skipped for order {ProviderOrderId}",
                payload.OrderId);

            return new AccesstradeWebhookResponse();
        }

        try
        {
            var orderResult = await _webhookProcessingService.ProcessAccesstradeAsync(
                payload,
                idempotencyResult.WebhookEventId,
                cancellationToken);

            await _idempotencyService.MarkAsProcessedAsync(
                idempotencyResult.WebhookEventId,
                cancellationToken);

            _logger.LogInformation(
                "Accesstrade webhook processed for order {ProviderOrderId}. Order {OrderId} created={WasCreated} updated={WasUpdated}",
                payload.OrderId,
                orderResult.OrderId,
                orderResult.WasCreated,
                orderResult.WasUpdated);

            return new AccesstradeWebhookResponse();
        }
        catch (Exception exception)
        {
            await _idempotencyService.MarkAsFailedAsync(
                idempotencyResult.WebhookEventId,
                exception.Message,
                cancellationToken);

            throw;
        }
    }
}
