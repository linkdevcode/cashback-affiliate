using System.Text.Json;
using Cashback.Api.Constants;
using Cashback.Api.Models;
using Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Webhook endpoints for affiliate provider integrations.
/// </summary>
[ApiController]
[Route("api/v1/webhooks")]
[AllowAnonymous]
public sealed class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the webhooks controller.
    /// </summary>
    public WebhooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Receives conversion postbacks from Accesstrade.
    /// </summary>
    [HttpPost("accesstrade")]
    [ProducesResponseType(typeof(ApiResponse<AccesstradeWebhookResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ProcessAccesstradeWebhook(
        [FromBody] AccesstradeWebhookRequest request,
        CancellationToken cancellationToken)
    {
        var rawPayload = ResolveRawPayload(request);

        var result = await _mediator.Send(
            new ProcessAccesstradeWebhookCommand(request, rawPayload),
            cancellationToken);

        return Ok(ApiResponse<AccesstradeWebhookResponse>.Ok(result));
    }

    /// <summary>
    /// Resolves the raw webhook payload from middleware context or serializes the request model.
    /// </summary>
    private string ResolveRawPayload(AccesstradeWebhookRequest request)
    {
        if (HttpContext.Items.TryGetValue(WebhookHttpContextKeys.RawBody, out var rawBody)
            && rawBody is string payload
            && !string.IsNullOrWhiteSpace(payload))
        {
            return payload;
        }

        return JsonSerializer.Serialize(request);
    }
}
