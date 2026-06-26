using Cashback.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cashback.Infrastructure.Webhooks;

/// <summary>
/// Security validator for Accesstrade webhook postbacks.
/// </summary>
public sealed class AccesstradeWebhookValidator : IWebhookValidator
{
    /// <summary>
    /// Accesstrade provider identifier.
    /// </summary>
    public const string ProviderName = "accesstrade";

    private readonly IAccesstradeWebhookSettings _webhookSettings;
    private readonly ILogger<AccesstradeWebhookValidator> _logger;

    /// <summary>
    /// Initializes a new instance of the Accesstrade webhook validator.
    /// </summary>
    public AccesstradeWebhookValidator(
        IAccesstradeWebhookSettings webhookSettings,
        ILogger<AccesstradeWebhookValidator> logger)
    {
        _webhookSettings = webhookSettings;
        _logger = logger;
    }

    /// <inheritdoc/>
    public string Provider => ProviderName;

    /// <inheritdoc/>
    public WebhookValidationResult Validate(WebhookValidationContext context)
    {
        var secretResult = ValidateSecret(context.Secret);
        if (!secretResult.IsValid)
        {
            return secretResult;
        }

        return ValidateSignature(context);
    }

    /// <summary>
    /// Validates the shared webhook secret when one is configured.
    /// </summary>
    private WebhookValidationResult ValidateSecret(string? providedSecret)
    {
        var configuredSecret = _webhookSettings.WebhookSecret;

        if (string.IsNullOrWhiteSpace(configuredSecret))
        {
            _logger.LogWarning(
                "Accesstrade webhook secret is not configured; skipping secret validation");
            return WebhookValidationResult.Success();
        }

        if (string.IsNullOrWhiteSpace(providedSecret)
            || !string.Equals(providedSecret, configuredSecret, StringComparison.Ordinal))
        {
            _logger.LogWarning("Accesstrade webhook rejected due to invalid secret");
            return WebhookValidationResult.Failure("Invalid webhook secret.");
        }

        return WebhookValidationResult.Success();
    }

    /// <summary>
    /// Validates the Accesstrade webhook signature.
    /// </summary>
    private WebhookValidationResult ValidateSignature(WebhookValidationContext context)
    {
        // TODO: Implement Accesstrade signature verification when official documentation is available.
        // Expected flow:
        // 1. Read signature from provider-specific header (confirm header name with Accesstrade).
        // 2. Compute HMAC-SHA256 of context.RawBody using WebhookSecret as the key.
        // 3. Compare computed digest with the supplied signature using a timing-safe comparison.
        // 4. Reject the request when the signature is missing or does not match.

        if (string.IsNullOrWhiteSpace(context.Signature))
        {
            _logger.LogDebug(
                "Accesstrade webhook signature header not present; signature validation skipped until provider spec is confirmed");
            return WebhookValidationResult.Success();
        }

        _logger.LogWarning(
            "Accesstrade webhook signature validation is not yet implemented; request accepted without signature verification");

        return WebhookValidationResult.Success();
    }
}
