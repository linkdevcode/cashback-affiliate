using Cashback.Api.Constants;
using Cashback.Api.Models;
using Cashback.Application.Interfaces;

namespace Cashback.Api.Middleware;

/// <summary>
/// Middleware that validates webhook request authenticity before controller execution.
/// </summary>
public sealed class WebhookSecurityMiddleware
{
    private const string WebhookPathPrefix = "/api/v1/webhooks";

    private readonly RequestDelegate _next;
    private readonly IReadOnlyDictionary<string, IWebhookValidator> _validators;
    private readonly ILogger<WebhookSecurityMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the webhook security middleware.
    /// </summary>
    public WebhookSecurityMiddleware(
        RequestDelegate next,
        IEnumerable<IWebhookValidator> validators,
        ILogger<WebhookSecurityMiddleware> logger)
    {
        _next = next;
        _validators = validators.ToDictionary(
            validator => validator.Provider,
            StringComparer.OrdinalIgnoreCase);
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware pipeline.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!ShouldValidate(context.Request))
        {
            await _next(context);
            return;
        }

        var provider = ResolveProvider(context.Request.Path);
        if (provider is null)
        {
            await _next(context);
            return;
        }

        if (!_validators.TryGetValue(provider, out var validator))
        {
            _logger.LogWarning(
                "Webhook validation failed: no validator registered for provider {Provider}",
                provider);
            await WriteUnauthorizedResponseAsync(context, "Unsupported webhook provider.");
            return;
        }

        var rawBody = await ReadRequestBodyAsync(context.Request);
        var validationContext = new WebhookValidationContext
        {
            Provider = provider,
            RawBody = rawBody,
            Secret = ResolveWebhookSecret(context.Request),
            Signature = ResolveWebhookSignature(context.Request),
            Path = context.Request.Path.Value ?? string.Empty
        };

        var validationResult = validator.Validate(validationContext);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Webhook validation failed for provider {Provider} on path {Path}: {Reason}",
                provider,
                validationContext.Path,
                validationResult.FailureReason);
            await WriteUnauthorizedResponseAsync(
                context,
                validationResult.FailureReason ?? "Webhook validation failed.");
            return;
        }

        context.Items[WebhookHttpContextKeys.RawBody] = rawBody;

        await _next(context);
    }

    /// <summary>
    /// Determines whether the current request targets a webhook endpoint.
    /// </summary>
    private static bool ShouldValidate(HttpRequest request)
    {
        return HttpMethods.IsPost(request.Method)
            && request.Path.StartsWithSegments(WebhookPathPrefix, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Extracts the provider name from the webhook request path.
    /// </summary>
    private static string? ResolveProvider(PathString path)
    {
        var segments = path.Value?
            .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (segments is null || segments.Length < 4)
        {
            return null;
        }

        // Expected: api / v1 / webhooks / {provider}
        if (!segments[0].Equals("api", StringComparison.OrdinalIgnoreCase)
            || !segments[1].Equals("v1", StringComparison.OrdinalIgnoreCase)
            || !segments[2].Equals("webhooks", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return segments[3];
    }

    /// <summary>
    /// Reads the request body while preserving it for downstream model binding.
    /// </summary>
    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }

    /// <summary>
    /// Resolves the webhook secret from supported request locations.
    /// </summary>
    private static string? ResolveWebhookSecret(HttpRequest request)
    {
        return request.Query["secret"].FirstOrDefault()
            ?? request.Headers["X-Webhook-Secret"].FirstOrDefault();
    }

    /// <summary>
    /// Resolves the webhook signature from supported request headers.
    /// </summary>
    private static string? ResolveWebhookSignature(HttpRequest request)
    {
        return request.Headers["X-Webhook-Signature"].FirstOrDefault()
            ?? request.Headers["X-Signature"].FirstOrDefault()
            ?? request.Headers["X-Accesstrade-Signature"].FirstOrDefault();
    }

    /// <summary>
    /// Writes a standardized unauthorized response for rejected webhook requests.
    /// </summary>
    private static async Task WriteUnauthorizedResponseAsync(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.Fail(message);
        await context.Response.WriteAsJsonAsync(response);
    }
}
