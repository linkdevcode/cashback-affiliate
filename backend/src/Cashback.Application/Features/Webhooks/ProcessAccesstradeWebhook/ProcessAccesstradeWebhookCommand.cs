using MediatR;

namespace Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;

/// <summary>
/// Command to process an incoming Accesstrade webhook postback.
/// </summary>
public sealed record ProcessAccesstradeWebhookCommand(
    AccesstradeWebhookRequest Payload,
    string RawPayload) : IRequest<AccesstradeWebhookResponse>;
